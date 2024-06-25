using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePlacer : MonoBehaviour
{
    // 장반경과 편평률 (WGS-84)
    private const double a = 6378137.0;
    private const double f = 1 / 298.257223563;
    private const double b = a * (1 - f);
    private const double e2 = 1 - (b / a) * (b / a);

    public Transform arObject; // 증강할 오브젝트
    public double myLatitude; // 내 위치의 위도
    public double myLongitude; // 내 위치의 경도
    public double targetLatitude; // 오브젝트의 위도
    public double targetLongitude; // 오브젝트의 경도

    private Vector3 referencePosition; // 기준점 (ECEF)

    void Start()
    {
        // 내 위치를 기준점으로 설정
        referencePosition = GPStoECEF(myLatitude, myLongitude);

        // 오브젝트의 위치를 계산
        var targetPosition = GPStoECEF(targetLatitude, targetLongitude);

        // ECEF 차이를 계산하여 Unity의 월드 좌표로 변환
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
