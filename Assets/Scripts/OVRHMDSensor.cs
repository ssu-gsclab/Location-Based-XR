using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OVRDisplayVelocity : MonoBehaviour
{
    public TextMeshProUGUI velocityText;

    void Start()
    {
        if (velocityText == null)
        {
            Debug.LogError("VelocityText is not assigned.");
        }
    }

    void Update()
    {
        // Get the linear and angular velocity from OVRDisplay
        Vector3 linearVelocity = OVRManager.display.velocity;
        Vector3 angularVelocity = OVRManager.display.angularVelocity;

        // Update the text UI with the velocity values
        if (velocityText != null)
        {
            velocityText.text = $"Linear Velocity: {linearVelocity}\nAngular Velocity: {angularVelocity}";
        }
    }
}

