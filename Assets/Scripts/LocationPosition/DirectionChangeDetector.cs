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

    public float alpha = 0.98f; // 필터 계수

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

        // Complementary Filter 적용
        filteredVelocity = alpha * (previousVelocity + currentVelocity * Time.deltaTime) + (1 - alpha) * currentVelocity;
        filteredAngularVelocity = alpha * (previousAngularVelocity + currentAngularVelocity * Time.deltaTime) + (1 - alpha) * currentAngularVelocity;

        // 현재 값 업데이트
        previousVelocity = filteredVelocity;
        previousAngularVelocity = filteredAngularVelocity;

        // 디버그 로그로 필터링된 값 확인
        Debug.Log("Filtered Velocity: " + filteredVelocity);
        Debug.Log("Filtered Angular Velocity: " + filteredAngularVelocity);

        // 사용자의 시선 방향이 크게 틀어졌는지 확인 (임계값 비교)
        CheckForDirectionChange(filteredVelocity, filteredAngularVelocity);
    }

    private void CheckForDirectionChange(Vector3 velocity, Vector3 angularVelocity)
    {
        // 예시 임계값 설정
        float velocityThreshold =1.0f;
        float angularVelocityThreshold = 0.5f;

        if (velocity.magnitude > velocityThreshold || angularVelocity.magnitude > angularVelocityThreshold)
        {
            Debug.Log("방향이 크게 틀어졌습니다!");
            // 필요한 로직 추가
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
