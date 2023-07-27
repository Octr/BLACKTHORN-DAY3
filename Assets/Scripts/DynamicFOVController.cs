using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynamicFOVController : MonoBehaviour
{
    public MovementController movementController;
    public float minFov = 50f;
    public float maxSpeedFov = 100f;
    public float fovChangeSpeed = 5f;

    private CinemachineVirtualCamera[] virtualCameras;

    void Start()
    {
        // Get all the CinemachineVirtualCamera components from the children
        virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>();
    }

    void Update()
    {
        // Get the current normalized speed from the movementController
        float speed01 = movementController.GetCurrentSpeed01();

        // Interpolate the desired FOV between minFov and maxSpeedFov
        float targetFov = Mathf.Lerp(minFov, maxSpeedFov, speed01);

        // Update the FOV of all CinemachineVirtualCamera objects
        foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
        {
            // Smoothly change the FOV
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * fovChangeSpeed);
        }
    }
}

