using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform followTarget;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float maxXRot = 100f;
    [SerializeField] private float minXRot = -90f;
    private float xRot, yRot;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);
    private Vector3 targetPosition;
    [SerializeField] private bool isCursorLocked = true;

    private bool isCamShaking;
    private float camShakeTimer = Mathf.Infinity;
    private float camShakeMagnitude;

    private void Update()
    {
        HandleCameraInput();

        UpdateCameraShake();

        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        AssignTargetPosition();
        FollowTarget();
    }

    private void HandleCameraInput()
    {
        float x = Input.GetAxisRaw("Mouse Y");
        float y = Input.GetAxisRaw("Mouse X");

        xRot -= x * sensitivity * Time.deltaTime;
        yRot += y * sensitivity * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, minXRot, maxXRot);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

    private void AssignTargetPosition()
    {
        if (followTarget == null) return;

        targetPosition = followTarget.position + offset;
    }

    private void FollowTarget()
    {
        transform.position = targetPosition;
    }

    private void UpdateCameraShake()
    {
        if (!isCamShaking) return;

        Vector3 randomShakeOffset = camShakeMagnitude * (Vector3)Random.insideUnitCircle.normalized;
        Vector3 targetShakePosition = targetPosition + randomShakeOffset;

        camShakeTimer -= Time.unscaledDeltaTime;
        if (camShakeTimer <= 0) isCamShaking = false;

        transform.position = Vector3.Lerp(transform.position, targetShakePosition, Time.unscaledDeltaTime);
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        isCamShaking = true;

        camShakeTimer = duration;
        camShakeMagnitude = magnitude;
    }
}
