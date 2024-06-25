using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToPosition : MonoBehaviour
{
    //public ObjectPlacer gpsManager;
    //public Transform arObjectPrefab;
    //public OVRCameraRig cameraRig;

    //private float initialHeading; // 초기 북쪽 방향

    //// 목표 위치 (예: 오브젝트를 배치할 위도와 경도)
    //public double targetLatitude; // 예시 위도
    //public double targetLongitude; // 예시 경도
    //public float earthRadius = 6371000f; // 지구 반경 (미터)

    //void Start()
    //{
    //    StartCoroutine(SetInitialHeadingAndPlaceObject());
    //}

    //IEnumerator SetInitialHeadingAndPlaceObject()
    //{
    //    // 5초 대기 후 초기 북쪽 방향 설정
    //    yield return new WaitForSeconds(5);
    //    initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
    //    Debug.Log("Initial heading set to: " + initialHeading);

    //    // 오브젝트 배치
    //    PlaceObjectAtTargetLocation();
    //}

    //void PlaceObjectAtTargetLocation()
    //{
    //    Vector3 position = GetPositionFromLocation(targetLatitude, targetLongitude);
    //    Instantiate(arObjectPrefab, position, Quaternion.identity);
    //}

    //private Vector3 GetPositionFromLocation(double targetLatitude, double targetLongitude)
    //{
    //    double referenceLatitude = gpsManager.latitude;
    //    double referenceLongitude = gpsManager.longitude;

    //    double latDiff = targetLatitude - referenceLatitude;
    //    double lonDiff = targetLongitude - referenceLongitude;

    //    double z = (latDiff * (Mathf.PI / 180) * earthRadius);
    //    double x = (lonDiff * (Mathf.PI / 180) * earthRadius * System.Math.Cos(referenceLatitude * Mathf.PI / 180));

    //    Vector3 position = new Vector3((float)x, 0, (float)z);

    //    return position;
    //}
    public ObjectPlacer gpsManager;
    public Transform arObjectPrefab;
    public OVRCameraRig cameraRig;

    // 초기 북쪽 방향
    private float initialHeading;

    // 목표 위치 (위도, 경도)
    public double targetLatitude;
    public double targetLongitude;
    public float earthRadius = 6371000f; // 지구 반경 (미터)

    void Start()
    {
        // Subscribe to the event in ProximityChecker
        ProximityChecker proximityChecker = FindObjectOfType<ProximityChecker>();
        if (proximityChecker == true)
        {
            StartCoroutine(SetInitialHeadingAndPlaceObject());
        }
    }

    //private void OnUserWithinDistance()
    //{
    //    // 5초 대기 후 초기 북쪽 방향 설정
    //    StartCoroutine(SetInitialHeadingAndPlaceObject());
    //}

    IEnumerator SetInitialHeadingAndPlaceObject()
    {
        // 5초 대기
        yield return new WaitForSeconds(5);

        // 초기 북쪽 방향 설정
        initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
        Debug.Log("Initial heading set to: " + initialHeading);

        // 오브젝트 배치
        PlaceObjectAtTargetLocation();
    }

    void PlaceObjectAtTargetLocation()
    {
        double referenceLatitude = gpsManager.latitude;
        double referenceLongitude = gpsManager.longitude;

        // 목표 위치와 현재 위치의 위도, 경도 차이 계산
        double latDiff = targetLatitude - referenceLatitude;
        double lonDiff = targetLongitude - referenceLongitude;

        // 지구 반경을 이용하여 x, z 값 계산
        double z = latDiff * (Mathf.PI / 180) * earthRadius;
        double x = lonDiff * (Mathf.PI / 180) * earthRadius * System.Math.Cos(referenceLatitude * Mathf.PI / 180);

        // 현재 위치와 목표 위치 방향 계산 (북쪽을 0도로 설정)
        //float targetHeading = GetHeadingFromTargetLocation(referenceLatitude, referenceLongitude, targetLatitude, targetLongitude);

        // 초기 북쪽 방향과 목표 위치 방향의 차이 계산
        //float headingDiff = targetHeading - initialHeading;

        // 회전할 각도 계산 (동쪽: 양수, 서쪽: 음수)
        //float rotationAngle = headingDiff;

        // 오브젝트 위치 계산
        Vector3 imagePosition = new Vector3((float)x, 0, (float)z);

        imagePosition = RotatePointAroundPivot(imagePosition, Vector3.zero, new Vector3(0, initialHeading, 0));

        // 사용자의 현재 위치
        Vector3 userPosition = new Vector3(0, 0, 0); // 기준점을 현재 위치로 설정

        // 회전 적용 (Quaternion.Euler 사용 시 상하좌우 회전이 동시에 일어남)
        //Quaternion rotation = Quaternion.Euler(0, 0, 0);

        // 방향을 계산하여 회전 적용
        Quaternion rotation = SetDirection(userPosition, imagePosition);

        // 오브젝트 배치
        Instantiate(arObjectPrefab, imagePosition, rotation); //rotation
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // pivot에서 point까지의 방향을 구합니다.
        dir = Quaternion.Euler(angles) * dir; // 방향을 회전시킵니다.
        point = dir + pivot; // 회전된 방향을 pivot에 더합니다.
        return point; // 최종 회전된 point를 반환합니다.
    }

    // 두 지점의 위도, 경도를 기반으로 방향 계산 (북쪽을 0도로 설정)
    private float GetHeadingFromTargetLocation(double currentLatitude, double currentLongitude, double targetLatitude, double targetLongitude)
    {
        double lonDiff = targetLongitude - currentLongitude;
        double y = System.Math.Sin(lonDiff) * System.Math.Cos(targetLatitude);
        double x = System.Math.Cos(currentLatitude) * System.Math.Sin(targetLatitude) - System.Math.Sin(currentLatitude) * System.Math.Cos(targetLatitude) * System.Math.Cos(lonDiff);

        // 아크탄젠트 함수를 이용하여 방향 계산 (북쪽: 0도, 동쪽: 90도, 서쪽: -90도, 남쪽: 180도)
        float heading = Mathf.Atan2((float)y, (float)x) * 180f / Mathf.PI;

        // 방향 값을 0~360도 범위로 변환
        if (heading < 0)
        {
            heading += 360f;
        }

        return heading;
    }

    private Quaternion SetDirection(Vector3 userLocation, Vector3 imageLocation)
    {

        Vector3 direction = userLocation - imageLocation;

        if (direction != Vector3.zero)
        {
            return Quaternion.LookRotation(direction);
        }
        else
        {
            return Quaternion.identity;
        }
    }
}
