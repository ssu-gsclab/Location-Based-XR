using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChecker : MonoBehaviour
{
    public double userLatitude = 37.4947727; // ������� ���� ����
    public double userLongitude = 126.9598034; // ������� ���� �浵

    public double targetLatitude = 37.4947488; // Ư�� ������ ����
    public double targetLongitude = 126.9594388; // Ư�� ������ �浵
    public float thresholdDistance = 150.0f; // �Ÿ� �Ӱ谪 (150m)

    void Update()
    {
        if (IsUserWithinDistance(userLatitude, userLongitude, targetLatitude, targetLongitude, thresholdDistance))
        {
            Debug.Log("User is within 150 meters of the target location.");
        }
        else
        {
            Debug.Log("User is not within 150 meters of the target location.");
        }
    }

    private bool IsUserWithinDistance(double userLat, double userLon, double targetLat, double targetLon, float maxDistance)
    {
        float earthRadius = 6371000f; // ���� �ݰ� (����)

        double dLat = Mathf.Deg2Rad * (targetLat - userLat);
        double dLon = Mathf.Deg2Rad * (targetLon - userLon);

        double a = System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2) +
                  System.Math.Cos(Mathf.Deg2Rad * userLat) * System.Math.Cos(Mathf.Deg2Rad * targetLat) *
                  System.Math.Sin(dLon / 2) * System.Math.Sin(dLon / 2);

        double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));

        double distance = earthRadius * c;

        return distance <= maxDistance;
    }
}
