using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform mainCamera; // 따라다닐 카메라
    public Vector3 offset = new Vector3(0, 0, 2);

    void Update()
    {
        if (mainCamera != null)
        {
            // 카메라의 위치와 회전을 따라가도록 설정
            transform.position = mainCamera.position + mainCamera.forward * offset.z + mainCamera.up * offset.y + mainCamera.right * offset.x;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        }
    }
}
