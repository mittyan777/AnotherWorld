using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    //�ړ���̖ڕW��ݒ肷��
    public Transform goal;
    private NavMeshAgent agent;
    private void Start()
    {
        
    }

    private void Update()
    {
        //NavMeshAgent�R���|�[�l���g���擾
        agent = GetComponent<NavMeshAgent>();

        //�ړ���̈ʒu��ݒ�
        if (goal != null)
        {
            agent.destination = goal.position;
        }
    }
}
