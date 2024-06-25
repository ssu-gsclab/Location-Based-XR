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
        // HMD의 속도와 각속도를 얻기
        velocity = OVRManager.display.velocity;
        angularVelocity = OVRManager.display.angularVelocity;
    }

    public Vector3 GetHMDForwardDirection(Transform hmdTransform)
    {
        // HMD의 전방 방향을 얻기
        return hmdTransform.forward;
    }
}
