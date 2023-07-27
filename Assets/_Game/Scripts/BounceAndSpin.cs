using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAndSpin : MonoBehaviour
{
    public Transform targetTransform;
    public float bounceHeight = 1.0f;
    public float bounceSpeed = 2.0f;
    public float spinSpeed = 180.0f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = targetTransform.position;
    }

    void Update()
    {
        Vector3 position = initialPosition;
        position.y += Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        targetTransform.position = position;
        targetTransform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
