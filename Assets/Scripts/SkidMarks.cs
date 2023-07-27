using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    TrailRenderer trailRenderer;
    public MovementController movementController;
    public Transform target;
    public float skidDotThreshold = 0.8f;
    public float skidSpeedThreshold = 2f;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        SetTrail(false);
    }

    void SetTrail(bool enabled)
    {
        trailRenderer.emitting = enabled;
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, .01f, target.position.z);
        // Check if the dot product is below the threshold
        if (Mathf.Abs(Input.GetAxis("Horizontal")) >= skidDotThreshold && movementController.GetSpeed() >= skidSpeedThreshold && movementController.IsGrounded() && transform.position.y < .12f)
        {
            SetTrail(true);
        }
        else
        {
            SetTrail(false);
        }
    }
}
