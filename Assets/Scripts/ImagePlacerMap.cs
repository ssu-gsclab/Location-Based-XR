using UnityEngine;

public class ImagePlacerMap : MonoBehaviour
{// WGS-84 Parameters
    private const double a = 6378137.0; // 장반경
    private const double f = 1 / 298.257223563; // 편평률
    private const double b = a * (1 - f); // 단반경
    private const double e2 = 1 - (b / a) * (b / a); // 제1편심률의 제곱

    public Transform arObject; // 증강할 오브젝트
    public double myLatitude; // 내 위치의 위도
    public double myLongitude; // 내 위치의 경도
    public double myAltitude; // 내 위치의 고도
    public double targetLatitude; // 오브젝트의 위도
    public double targetLongitude; // 오브젝트의 경도
    public double targetAltitude; // 오브젝트의 고도

    public Transform centerEyeAnchor; // OVRCameraRig의 CenterEyeAnchor
    //public Transform mapParent; // 지도를 포함하는 부모 객체

    private Vector3 referencePosition; // 기준점 (ECEF)
    private Quaternion initialRotation; // 초기 회전값
    private bool isInitialRotationCaptured = false;

    void Start()
    {
        // 내 위치를 기준점으로 설정
        referencePosition = GPStoECEF(myLatitude, myLongitude, myAltitude);

        // 10초 후에 초기 회전을 북쪽으로 설정
        Invoke("CaptureInitialRotation", 10.0f);
    }

    void Update()
    {
        if (isInitialRotationCaptured)
        {
            // 오브젝트의 위치를 계산
            var targetPosition = GPStoECEF(targetLatitude, targetLongitude, targetAltitude);

            // ECEF 차이를 계산
            Vector3 ecefDifference = targetPosition - referencePosition;

            // ECEF를 ENU로 변환
            Vector3 enuPosition = ECEFtoENU(ecefDifference, myLatitude, myLongitude);

            // Unity의 월드 좌표로 변환하여 오브젝트 배치
            arObject.position = enuPosition;

            // 방향 적용 (원하는 경우 추가 가능)
            arObject.rotation = CalculateTargetRotation(enuPosition);
        }
    }

    Vector3 GPStoECEF(double lat, double lon, double alt)
    {
        double latRad = Mathf.Deg2Rad * lat;
        double lonRad = Mathf.Deg2Rad * lon;

        double N = a / System.Math.Sqrt(1 - e2 * System.Math.Sin((float)latRad) * System.Math.Sin((float)latRad));

        double x = (N + alt) * Mathf.Cos((float)latRad) * Mathf.Cos((float)lonRad);
        double y = (N + alt) * Mathf.Cos((float)latRad) * Mathf.Sin((float)lonRad);
        double z = ((1 - e2) * N + alt) * Mathf.Sin((float)latRad);

        return new Vector3((float)x, (float)y, (float)z);
    }

    Vector3 ECEFtoENU(Vector3 ecef, double refLat, double refLon)
    {
        double latRad = Mathf.Deg2Rad * refLat;
        double lonRad = Mathf.Deg2Rad * refLon;

        // ENU 변환 행렬
        float sinLat = Mathf.Sin((float)latRad);
        float cosLat = Mathf.Cos((float)latRad);
        float sinLon = Mathf.Sin((float)lonRad);
        float cosLon = Mathf.Cos((float)lonRad);

        float t = cosLon * ecef.x + sinLon * ecef.y;
        float x = -sinLon * ecef.x + cosLon * ecef.y;
        float y = -sinLat * t + cosLat * ecef.z;
        float z = cosLat * t + sinLat * ecef.z;

        return new Vector3(x, y, z);
    }

    void CaptureInitialRotation()
    {
        initialRotation = Quaternion.Euler(0, centerEyeAnchor.eulerAngles.y, 0);
        isInitialRotationCaptured = true;

        // 사용자가 바라보는 방향을 북쪽으로 설정하기 위해 지도 회전
        //mapParent.rotation = Quaternion.Inverse(initialRotation);
    }

    Quaternion CalculateTargetRotation(Vector3 relativePosition)
    {
        // 방향 벡터를 이용하여 회전값을 계산
        Vector3 directionToTarget = relativePosition.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        Quaternion adjustedRotation = initialRotation * Quaternion.Inverse(targetRotation);

        return adjustedRotation;
    }
}
