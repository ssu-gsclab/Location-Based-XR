using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionChangeDetector : MonoBehaviour
{
    public Transform hmdTransform;
    private Vector3 previousVelocity;
    private Vector3 previousAngularVelocity;

    private Vector3 filteredVelocity;
    private Vector3 filteredAngularVelocity;

    public float alpha = 0.98f; // ���� ���

    void Start()
    {
        previousVelocity = OVRManager.display.velocity;
        previousAngularVelocity = OVRManager.display.angularVelocity;
        filteredVelocity = previousVelocity;
        filteredAngularVelocity = previousAngularVelocity;
    }

    void Update()
    {
        Vector3 currentVelocity = OVRManager.display.velocity;
        Vector3 currentAngularVelocity = OVRManager.display.angularVelocity;

        // Complementary Filter ����
        filteredVelocity = alpha * (previousVelocity + currentVelocity * Time.deltaTime) + (1 - alpha) * currentVelocity;
        filteredAngularVelocity = alpha * (previousAngularVelocity + currentAngularVelocity * Time.deltaTime) + (1 - alpha) * currentAngularVelocity;

        // ���� �� ������Ʈ
        previousVelocity = filteredVelocity;
        previousAngularVelocity = filteredAngularVelocity;

        // ����� �α׷� ���͸��� �� Ȯ��
        Debug.Log("Filtered Velocity: " + filteredVelocity);
        Debug.Log("Filtered Angular Velocity: " + filteredAngularVelocity);

        // ������� �ü� ������ ũ�� Ʋ�������� Ȯ�� (�Ӱ谪 ��)
        CheckForDirectionChange(filteredVelocity, filteredAngularVelocity);
    }

    private void CheckForDirectionChange(Vector3 velocity, Vector3 angularVelocity)
    {
        // ���� �Ӱ谪 ����
        float velocityThreshold =1.0f;
        float angularVelocityThreshold = 0.5f;

        if (velocity.magnitude > velocityThreshold || angularVelocity.magnitude > angularVelocityThreshold)
        {
            Debug.Log("������ ũ�� Ʋ�������ϴ�!");
            // �ʿ��� ���� �߰�
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
