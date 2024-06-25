using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class ARImagePlacer : MonoBehaviour
{
    public Transform arObjectPrefab; // 증강할 AR 오브젝트 프리팹
    public Transform hmdTransform;

    public double currentLat;
    public double currentLon;
    public double photoLat;
    public double photoLon;
    public float currentHeight = 0.0f;

    //private Transform arObjectInstance;


    void Start()
    {
        currentLat = GetLocation.Instance.latitude_init;
        currentLon = GetLocation.Instance.longitude_init;

        // 사진의 위치를 계산
        Vector3 photoPosition = LocationUtils.CalculatePositionFromLocation(currentLat, currentLon, photoLat, photoLon, currentHeight);

        // AR 오브젝트를 생성하고 위치 설정
        Transform arObject = Instantiate(arObjectPrefab, photoPosition, Quaternion.identity);
        

        // 사용자의 시선 방향을 설정
        Vector3 forwardDirection = GetHMDForwardDirection(hmdTransform);
        arObject.rotation = Quaternion.LookRotation(forwardDirection);
    }

    Vector3 GetHMDForwardDirection(Transform hmdTransform)
    {
        return hmdTransform.forward;
    }
}
