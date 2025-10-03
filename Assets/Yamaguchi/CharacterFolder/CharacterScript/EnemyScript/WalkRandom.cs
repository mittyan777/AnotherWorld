using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkRandom : MonoBehaviour
{
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        NextDestination();
    }

    private void NextDestination() 
    {
        var randomPos = new Vector3(Random.Range(0, 60), 0, Random.Range(0, 60));
        agent.destination = randomPos;
    }
    private void Update()
    {
        if (agent.remainingDistance < 0.5f) 
        { 
            NextDestination();
        }
    }

}
