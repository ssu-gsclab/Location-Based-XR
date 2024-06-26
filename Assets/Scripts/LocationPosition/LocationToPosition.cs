using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToPosition : MonoBehaviour
{
    public GPSManager gpsManager;
    public Transform arObjectPrefab;
    public OVRCameraRig cameraRig;

    // �ʱ� ���� ����
    private float initialHeading;

    // ��ǥ ��ġ (����, �浵)
    public double targetLatitude;
    public double targetLongitude;
    public float earthRadius = 6371000f; // ���� �ݰ� (����)

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
        // 5�� ���
        yield return new WaitForSeconds(5);

        // �ʱ� ���� ���� ����
        initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
        Debug.Log("Initial heading set to: " + initialHeading);

        // ������Ʈ ��ġ
        PlaceImageAtTargetLocation();
    }

    void PlaceImageAtTargetLocation()
    {
        double referenceLatitude = gpsManager.latitude;
        double referenceLongitude = gpsManager.longitude;

        // ��ǥ ��ġ�� ���� ��ġ�� ����, �浵 ���� ���
        double latDiff = targetLatitude - referenceLatitude;
        double lonDiff = targetLongitude - referenceLongitude;

        // ���� �ݰ��� �̿��Ͽ� x, z �� ���
        double z = latDiff * (Mathf.PI / 180) * earthRadius;
        double x = lonDiff * (Mathf.PI / 180) * earthRadius * System.Math.Cos(referenceLatitude * Mathf.PI / 180);

        // ������Ʈ ��ġ ���
        Vector3 imagePosition = new Vector3((float)x, 0, (float)z);

        imagePosition = RotatePointAroundPivot(imagePosition, Vector3.zero, new Vector3(0, initialHeading, 0));

        // ������� ���� ��ġ
        Vector3 userPosition = new Vector3(0, 0, 0); // �������� ���� ��ġ�� ����

        // ������ ����Ͽ� ȸ�� ����
        Quaternion rotation = SetRotation(userPosition, imagePosition);

        // ������Ʈ ��ġ
        Instantiate(arObjectPrefab, imagePosition, rotation); //rotation
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // pivot���� point������ ����
        dir = Quaternion.Euler(angles) * dir; // ������ ȸ��
        point = dir + pivot; // ȸ���� ���� + pivot
        return point; // ���� ȸ���� point�� ��ȯ
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
