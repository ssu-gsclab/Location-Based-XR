using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class ARImagePlacer : MonoBehaviour
{
    public Transform arObjectPrefab; // ������ AR ������Ʈ ������
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

        // ������ ��ġ�� ���
        Vector3 photoPosition = LocationUtils.CalculatePositionFromLocation(currentLat, currentLon, photoLat, photoLon, currentHeight);

        // AR ������Ʈ�� �����ϰ� ��ġ ����
        Transform arObject = Instantiate(arObjectPrefab, photoPosition, Quaternion.identity);
        

        // ������� �ü� ������ ����
        Vector3 forwardDirection = GetHMDForwardDirection(hmdTransform);
        arObject.rotation = Quaternion.LookRotation(forwardDirection);
    }

    Vector3 GetHMDForwardDirection(Transform hmdTransform)
    {
        return hmdTransform.forward;
    }
}
