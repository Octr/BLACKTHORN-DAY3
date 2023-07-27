using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostZone : MonoBehaviour
{
    public float boostStrength;

    private void OnTriggerEnter(Collider other)
    {
        MovementController mc = other.GetComponent<MovementController>();
        if (mc)
        {
            Vector3 boostDirection = transform.forward;

            mc.AddExternalForce(boostDirection * boostStrength);
            mc.currentSpeed *= boostStrength;
        }
    }
}
