using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARImageController : MonoBehaviour
{
    public Transform arObject; // AR 오브젝트 (큐브)

    private Vector3 angle = Vector3.zero;
    public float alpha = 0.98f; // Complementary filter coefficient

    void Update()
    {
        // Time delta
        float dt = Time.deltaTime;

        // Get the angular velocity from OVRDisplay
        Vector3 angularVelocity = OVRManager.display.angularVelocity;

        // Calculate the angle from angular velocity (gyro)
        Vector3 gyroAngle = angularVelocity * dt;

        // Simulate accelerometer angle (this should be replaced with actual accelerometer data if available)
        Vector3 accelAngle = new Vector3(
            Mathf.Atan2(OVRManager.display.velocity.y, OVRManager.display.velocity.z) * Mathf.Rad2Deg,
            Mathf.Atan2(OVRManager.display.velocity.x, OVRManager.display.velocity.z) * Mathf.Rad2Deg,
            Mathf.Atan2(OVRManager.display.velocity.x, OVRManager.display.velocity.y) * Mathf.Rad2Deg
        );

        // Apply the complementary filter
        angle = alpha * (angle + gyroAngle) + (1 - alpha) * accelAngle;

        // Use the filtered angle to update the AR object's rotation
        if (arObject != null)
        {
            arObject.rotation = Quaternion.Euler(angle);
        }
    }
}
