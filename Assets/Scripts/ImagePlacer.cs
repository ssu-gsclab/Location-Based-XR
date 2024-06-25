using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePlacer : MonoBehaviour
{
    // ��ݰ�� ����� (WGS-84)
    private const double a = 6378137.0;
    private const double f = 1 / 298.257223563;
    private const double b = a * (1 - f);
    private const double e2 = 1 - (b / a) * (b / a);

    public Transform arObject; // ������ ������Ʈ
    public double myLatitude; // �� ��ġ�� ����
    public double myLongitude; // �� ��ġ�� �浵
    public double targetLatitude; // ������Ʈ�� ����
    public double targetLongitude; // ������Ʈ�� �浵

    private Vector3 referencePosition; // ������ (ECEF)

    void Start()
    {
        // �� ��ġ�� ���������� ����
        referencePosition = GPStoECEF(myLatitude, myLongitude);

        // ������Ʈ�� ��ġ�� ���
        var targetPosition = GPStoECEF(targetLatitude, targetLongitude);

        // ECEF ���̸� ����Ͽ� Unity�� ���� ��ǥ�� ��ȯ
        Vector3 relativePosition = targetPosition - referencePosition;
        arObject.position = relativePosition;
        Debug.Log(arObject.position);
    }

    Vector3 GPStoECEF(double lat, double lon)
    {
        double latRad = System.Math.PI / 180.0 * lat;
        double lonRad = System.Math.PI / 180.0 * lon;

        double N = a / System.Math.Sqrt(1 - e2 * System.Math.Sin(latRad) * System.Math.Sin(latRad));

        double x = N * System.Math.Cos(latRad) * System.Math.Cos(lonRad);
        double y = N * System.Math.Cos(latRad) * System.Math.Sin(lonRad);
        double z = ((b * b / (a * a)) * N) * System.Math.Sin(latRad);

        return new Vector3((float)x, (float)y, (float)z);
    }
}
