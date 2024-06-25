using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform mainCamera; // ����ٴ� ī�޶�
    public Vector3 offset = new Vector3(0, 0, 2);

    void Update()
    {
        if (mainCamera != null)
        {
            // ī�޶��� ��ġ�� ȸ���� ���󰡵��� ����
            transform.position = mainCamera.position + mainCamera.forward * offset.z + mainCamera.up * offset.y + mainCamera.right * offset.x;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        }
    }
}
