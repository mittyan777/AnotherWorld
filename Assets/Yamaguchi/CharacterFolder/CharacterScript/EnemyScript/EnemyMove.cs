using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    /*
    //�ړ���̖ڕW��ݒ肷��
    public Transform goal;
    private NavMeshAgent agent;
    private void Start()
    {
        
    }

    private void Update()
    {
        ////NavMeshAgent�R���|�[�l���g���擾
        //agent = GetComponent<NavMeshAgent>();

        ////�ړ���̈ʒu��ݒ�
        //if (goal != null)
        //{
        //    agent.destination = goal.position;
        //}
    }*/
    private NavMeshAgent agent;
    
    [SerializeField]private float stayDuration;
    private float timer;
    private bool isWaiting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextDestination();
    }

    private void NextDestination()
    {
        var randomPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
        agent.destination = randomPos;
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            timer += Time.deltaTime;
            if (timer >= stayDuration)
            { 
                NextDestination();
                timer = 0;
            }
        }
    }
}
