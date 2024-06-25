using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDTracker : MonoBehaviour
{
    public Transform hmdTransform;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    void Update()
    {
        // HMD�� �ӵ��� ���ӵ��� ���
        velocity = OVRManager.display.velocity;
        angularVelocity = OVRManager.display.angularVelocity;
    }

    public Vector3 GetHMDForwardDirection(Transform hmdTransform)
    {
        // HMD�� ���� ������ ���
        return hmdTransform.forward;
    }
}
