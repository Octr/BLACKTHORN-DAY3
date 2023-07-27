using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    MovementController player;


    public Transform direction;
    public float maxForce = 5f;
    public float minForce = 1f;

    public float maxDistance = 10f;

    private void FixedUpdate()
    {
        if (player) {
            player.AddExternalForce(direction.forward * Mathf.Lerp(maxForce, minForce, Mathf.Clamp01(Vector3.Distance(transform.position, player.transform.position) / maxDistance)));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MovementController mc = other.GetComponent<MovementController>();
        if (mc)
        {
            player = mc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MovementController mc = other.GetComponent<MovementController>();
        if (mc)
        {
            player = null;
        }
    }

}
