using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreValue = 1000;
    public bool isBuilding;

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
            if(mc.GetSpeed() > 200)
            {
                Game.Instance.GetDestructionEffect(transform.position);
                Game.Instance.AddScore(scoreValue);
                ScreenshakeManager.Instance.ApplyScreenshake();
                gameObject.SetActive(false);
            } else
            {
                if(isBuilding)
                {
                    rb.AddForceAtPosition(mc.GetVelocity() * 10f, collision.contacts[0].point);
                }
                else
                {
                    rb.AddForceAtPosition(mc.GetVelocity() * 4f, collision.contacts[0].point);
                }

                
            }
        }
    }
}
