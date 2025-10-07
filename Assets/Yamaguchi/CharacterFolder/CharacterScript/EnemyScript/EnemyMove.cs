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
    [SerializeField] private float stayDuration;
    private float timer;

    //================�ړ���ύX(�v���C���[)================
    //�v���C���[�����F�ł��鋗��
    [SerializeField] private float visibilityDistance;
    [SerializeField] private Transform playerTransform;
    
    private bool isStoppedByAttack = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        { 
            playerTransform = player.transform;
        }
        NextDestination();
    }

    private void NextDestination()
    {
        if (agent.enabled && !agent.isStopped)
        {
            var randomPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
            agent.destination = randomPos;
        }        
    }

    private void Update()
    {
        if (isStoppedByAttack) return;

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= visibilityDistance) // �w��͈͓��ɓ����Ă��邩
            {
                // �v���C���[��ǐ�
                agent.destination = playerTransform.position;
            }
            else
            {
                //�v���C���[�������Ȃ��A�܂��͉�������ꍇ�͏��񃍃W�b�N��
                PatrolLogic();
            }
        }
        else
        {
            // �v���C���[Transform���ݒ肳��Ă��Ȃ��ꍇ�����񃍃W�b�N��
            PatrolLogic();
        }
    }

    private void PatrolLogic()
    {
        // �ړI�n�ɓ��B�������`�F�b�N
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            timer += Time.deltaTime;

            if (timer >= stayDuration)
            {
                NextDestination();
                timer = 0;
            }
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            //�ĊJ�� (isAttacking��false�ɂȂ����Ƃ�) �̂݁A�����ɖړI�n���X�V
            if (!isAttacking)
            {
                //�v���C���[�������Ă���ΒǐՁA�����Ă��Ȃ���Ώ�����ĊJ
                if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= visibilityDistance)
                {
                    agent.SetDestination(playerTransform.position);
                }
                else
                {
                    NextDestination();
                }
            }
        }
    }
}
