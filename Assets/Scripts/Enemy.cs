using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreValue = 1000;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION with " + collision.gameObject.name);
        MovementController mc = collision.gameObject.GetComponent<MovementController>();
        if (mc)
        {
            if(mc.GetSpeed() > 100)
            {
                Game.Instance.GetDestructionEffect(transform.position);
                Game.Instance.AddScore(scoreValue);
                gameObject.SetActive(false);
            } else
            {
                rb.AddForceAtPosition(mc.GetVelocity() * 2f, collision.contacts[0].point);
            }
        }
    }
}
