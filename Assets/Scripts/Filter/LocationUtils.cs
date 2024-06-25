using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationUtils : MonoBehaviour
{
    private const double EarthRadius = 6371000; // 지구 반경 (미터)

    // 위도와 경도로부터 두 점 사이의 거리를 계산
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadius * c;
    }

    // 라디안으로 변환
    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    // 사진의 위치를 계산 (현재 위치에서 특정 거리와 방향으로 이동한 위치)
    public static Vector3 CalculatePositionFromLocation(double currentLat, double currentLon, double photoLat, double photoLon, float currentHeight)
    {
        double distance = CalculateDistance(currentLat, currentLon, photoLat, photoLon);
        double bearing = GetBearing(currentLat, currentLon, photoLat, photoLon);

        float x = (float)(distance * Math.Cos(bearing));
        float z = (float)(distance * Math.Sin(bearing));

        return new Vector3(x, currentHeight, z);
    }

    // 방위각 계산
    private static double GetBearing(double lat1, double lon1, double lat2, double lon2)
    {
        double dLon = DegreesToRadians(lon2 - lon1);
        double y = Math.Sin(dLon) * Math.Cos(DegreesToRadians(lat2));
        double x = Math.Cos(DegreesToRadians(lat1)) * Math.Sin(DegreesToRadians(lat2)) -
                   Math.Sin(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) * Math.Cos(dLon);
        return Math.Atan2(y, x);
    }
}
