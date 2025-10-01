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
    //================�ҋ@�p�ϐ�================
    [SerializeField]private float stayDuration;
    private float timer;
    private bool isWaiting = false;

    //================�ړ���ύX(�v���C���[)================
    //�v���C���[�����F�ł��鋗��
    [SerializeField] private float visibilityDistance;
    [SerializeField] private Transform playerTransform;
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
        float disutanceX = playerTransform.position.x - transform.position.x;
        float disutanceZ = playerTransform.position.z - transform.position.z;

        if (visibilityDistance < disutanceX || visibilityDistance < disutanceZ)
        {
            agent.destination = playerTransform.position;
        }
        else
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
}
