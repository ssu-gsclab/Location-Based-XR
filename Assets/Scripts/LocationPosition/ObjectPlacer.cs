using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public double latitude;
    public double longitude;

    void Start()
    {
        // 사용자 위치를 코드에서 직접 설정
        latitude = 37.4947123; // 예시 위도
        longitude = 126.9596691; // 예시 경도
    }
}
