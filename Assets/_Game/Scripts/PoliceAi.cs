using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAi : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    public Transform target;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
       agent.SetDestination(target.position);
    } 
}
