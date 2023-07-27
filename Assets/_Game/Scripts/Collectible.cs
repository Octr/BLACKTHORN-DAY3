using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MovementController mc = other.GetComponent<MovementController>();
        if (mc)
        {
            Game.Instance.AddScore(100);
            gameObject.SetActive(false);
        }
    }
}
