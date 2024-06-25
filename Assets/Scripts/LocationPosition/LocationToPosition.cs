using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToPosition : MonoBehaviour
{
    //public ObjectPlacer gpsManager;
    //public Transform arObjectPrefab;
    //public OVRCameraRig cameraRig;

    //private float initialHeading; // �ʱ� ���� ����

    //// ��ǥ ��ġ (��: ������Ʈ�� ��ġ�� ������ �浵)
    //public double targetLatitude; // ���� ����
    //public double targetLongitude; // ���� �浵
    //public float earthRadius = 6371000f; // ���� �ݰ� (����)

    //void Start()
    //{
    //    StartCoroutine(SetInitialHeadingAndPlaceObject());
    //}

    //IEnumerator SetInitialHeadingAndPlaceObject()
    //{
    //    // 5�� ��� �� �ʱ� ���� ���� ����
    //    yield return new WaitForSeconds(5);
    //    initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
    //    Debug.Log("Initial heading set to: " + initialHeading);

    //    // ������Ʈ ��ġ
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
            StartCoroutine(SetInitialHeadingAndPlaceObject());
        }
    }

    //private void OnUserWithinDistance()
    //{
    //    // 5�� ��� �� �ʱ� ���� ���� ����
    //    StartCoroutine(SetInitialHeadingAndPlaceObject());
    //}

    IEnumerator SetInitialHeadingAndPlaceObject()
    {
        // 5�� ���
        yield return new WaitForSeconds(5);

        // �ʱ� ���� ���� ����
        initialHeading = cameraRig.centerEyeAnchor.eulerAngles.y;
        Debug.Log("Initial heading set to: " + initialHeading);

        // ������Ʈ ��ġ
        PlaceObjectAtTargetLocation();
    }

    void PlaceObjectAtTargetLocation()
    {
        double referenceLatitude = gpsManager.latitude;
        double referenceLongitude = gpsManager.longitude;

        // ��ǥ ��ġ�� ���� ��ġ�� ����, �浵 ���� ���
        double latDiff = targetLatitude - referenceLatitude;
        double lonDiff = targetLongitude - referenceLongitude;

        // ���� �ݰ��� �̿��Ͽ� x, z �� ���
        double z = latDiff * (Mathf.PI / 180) * earthRadius;
        double x = lonDiff * (Mathf.PI / 180) * earthRadius * System.Math.Cos(referenceLatitude * Mathf.PI / 180);

        // ���� ��ġ�� ��ǥ ��ġ ���� ��� (������ 0���� ����)
        //float targetHeading = GetHeadingFromTargetLocation(referenceLatitude, referenceLongitude, targetLatitude, targetLongitude);

        // �ʱ� ���� ����� ��ǥ ��ġ ������ ���� ���
        //float headingDiff = targetHeading - initialHeading;

        // ȸ���� ���� ��� (����: ���, ����: ����)
        //float rotationAngle = headingDiff;

        // ������Ʈ ��ġ ���
        Vector3 imagePosition = new Vector3((float)x, 0, (float)z);

        imagePosition = RotatePointAroundPivot(imagePosition, Vector3.zero, new Vector3(0, initialHeading, 0));

        // ������� ���� ��ġ
        Vector3 userPosition = new Vector3(0, 0, 0); // �������� ���� ��ġ�� ����

        // ȸ�� ���� (Quaternion.Euler ��� �� �����¿� ȸ���� ���ÿ� �Ͼ)
        //Quaternion rotation = Quaternion.Euler(0, 0, 0);

        // ������ ����Ͽ� ȸ�� ����
        Quaternion rotation = SetDirection(userPosition, imagePosition);

        // ������Ʈ ��ġ
        Instantiate(arObjectPrefab, imagePosition, rotation); //rotation
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // pivot���� point������ ������ ���մϴ�.
        dir = Quaternion.Euler(angles) * dir; // ������ ȸ����ŵ�ϴ�.
        point = dir + pivot; // ȸ���� ������ pivot�� ���մϴ�.
        return point; // ���� ȸ���� point�� ��ȯ�մϴ�.
    }

    // �� ������ ����, �浵�� ������� ���� ��� (������ 0���� ����)
    private float GetHeadingFromTargetLocation(double currentLatitude, double currentLongitude, double targetLatitude, double targetLongitude)
    {
        double lonDiff = targetLongitude - currentLongitude;
        double y = System.Math.Sin(lonDiff) * System.Math.Cos(targetLatitude);
        double x = System.Math.Cos(currentLatitude) * System.Math.Sin(targetLatitude) - System.Math.Sin(currentLatitude) * System.Math.Cos(targetLatitude) * System.Math.Cos(lonDiff);

        // ��ũź��Ʈ �Լ��� �̿��Ͽ� ���� ��� (����: 0��, ����: 90��, ����: -90��, ����: 180��)
        float heading = Mathf.Atan2((float)y, (float)x) * 180f / Mathf.PI;

        // ���� ���� 0~360�� ������ ��ȯ
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
