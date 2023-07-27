using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform target;
    public bool isWaiting;
    public float cooldown = 3f;

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
        if(isWaiting) return;

        agent.SetDestination(target.position);
    }

    public IEnumerator Cooldown()
    {
        isWaiting = true;
        yield return new WaitForSeconds(cooldown);
        isWaiting = false;
    }
}
