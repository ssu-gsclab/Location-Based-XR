using UnityEngine;

public class ImagePlacerMap : MonoBehaviour
{// WGS-84 Parameters
    private const double a = 6378137.0; // ��ݰ�
    private const double f = 1 / 298.257223563; // �����
    private const double b = a * (1 - f); // �ܹݰ�
    private const double e2 = 1 - (b / a) * (b / a); // ��1��ɷ��� ����

    public Transform arObject; // ������ ������Ʈ
    public double myLatitude; // �� ��ġ�� ����
    public double myLongitude; // �� ��ġ�� �浵
    public double myAltitude; // �� ��ġ�� ��
    public double targetLatitude; // ������Ʈ�� ����
    public double targetLongitude; // ������Ʈ�� �浵
    public double targetAltitude; // ������Ʈ�� ��

    public Transform centerEyeAnchor; // OVRCameraRig�� CenterEyeAnchor
    //public Transform mapParent; // ������ �����ϴ� �θ� ��ü

    private Vector3 referencePosition; // ������ (ECEF)
    private Quaternion initialRotation; // �ʱ� ȸ����
    private bool isInitialRotationCaptured = false;

    void Start()
    {
        // �� ��ġ�� ���������� ����
        referencePosition = GPStoECEF(myLatitude, myLongitude, myAltitude);

        // 10�� �Ŀ� �ʱ� ȸ���� �������� ����
        Invoke("CaptureInitialRotation", 10.0f);
    }

    void Update()
    {
        if (isInitialRotationCaptured)
        {
            // ������Ʈ�� ��ġ�� ���
            var targetPosition = GPStoECEF(targetLatitude, targetLongitude, targetAltitude);

            // ECEF ���̸� ���
            Vector3 ecefDifference = targetPosition - referencePosition;

            // ECEF�� ENU�� ��ȯ
            Vector3 enuPosition = ECEFtoENU(ecefDifference, myLatitude, myLongitude);

            // Unity�� ���� ��ǥ�� ��ȯ�Ͽ� ������Ʈ ��ġ
            arObject.position = enuPosition;

            // ���� ���� (���ϴ� ��� �߰� ����)
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

        // ENU ��ȯ ���
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

        // ����ڰ� �ٶ󺸴� ������ �������� �����ϱ� ���� ���� ȸ��
        //mapParent.rotation = Quaternion.Inverse(initialRotation);
    }

    Quaternion CalculateTargetRotation(Vector3 relativePosition)
    {
        // ���� ���͸� �̿��Ͽ� ȸ������ ���
        Vector3 directionToTarget = relativePosition.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        Quaternion adjustedRotation = initialRotation * Quaternion.Inverse(targetRotation);

        return adjustedRotation;
    }
}
