using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToPosition : MonoBehaviour
{
    public GPSManager gpsManager;
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
            StartCoroutine(SetInitialHeading());
        }
    }

    IEnumerator SetInitialHeading()
    {
        // 5초 대기
        yield return new WaitForSeconds(5);

        // 초기 북쪽 방향 설정
        initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
        Debug.Log("Initial heading set to: " + initialHeading);

        // 오브젝트 배치
        PlaceImageAtTargetLocation();
    }

    void PlaceImageAtTargetLocation()
    {
        double referenceLatitude = gpsManager.latitude;
        double referenceLongitude = gpsManager.longitude;

        // 목표 위치와 현재 위치의 위도, 경도 차이 계산
        double latDiff = targetLatitude - referenceLatitude;
        double lonDiff = targetLongitude - referenceLongitude;

        // 지구 반경을 이용하여 x, z 값 계산
        double z = latDiff * (Mathf.PI / 180) * earthRadius;
        double x = lonDiff * (Mathf.PI / 180) * earthRadius * System.Math.Cos(referenceLatitude * Mathf.PI / 180);

        // 오브젝트 위치 계산
        Vector3 imagePosition = new Vector3((float)x, 0, (float)z);

        imagePosition = RotatePointAroundPivot(imagePosition, Vector3.zero, new Vector3(0, initialHeading, 0));

        // 사용자의 현재 위치
        Vector3 userPosition = new Vector3(0, 0, 0); // 기준점을 현재 위치로 설정

        // 방향을 계산하여 회전 적용
        Quaternion rotation = SetRotation(userPosition, imagePosition);

        // 오브젝트 배치
        Instantiate(arObjectPrefab, imagePosition, rotation); //rotation
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // pivot에서 point까지의 방향
        dir = Quaternion.Euler(angles) * dir; // 방향을 회전
        point = dir + pivot; // 회전된 방향 + pivot
        return point; // 최종 회전된 point를 반환
    }

    private Quaternion SetRotation(Vector3 userLocation, Vector3 imageLocation)
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
