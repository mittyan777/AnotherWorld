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

    //=========�G�̏�Ԃ�\���t���O(�A�j���[�V�����p)=========
    public enum EnemyState { Idle, Walk, Attack, None }
    public EnemyState currentState = EnemyState.Idle;

    [SerializeField] EnemyAnimationManager animationManager;

    private bool isStoppedByAttack = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationManager = GetComponent<EnemyAnimationManager>();

        
        if (animationManager == null)
        {
            animationManager = GetComponentInChildren<EnemyAnimationManager>();
            Debug.LogError("EnemyAnimationManager���A�^�b�`����Ă��܂���B");
        }
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
        if (animationManager == null)
        {

        }
        else
        {
            // Manager�������99�s�ڂ̏��������s
            animationManager.UpdateAnimation(currentState);
        }

        if (isStoppedByAttack) 
        {
            //�A�j���[�V�����J�ڗp�̃t���O�ύX
            currentState = EnemyState.Attack;
            return;
        } 

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            currentState = EnemyState.Walk;
            //�A�j���[�V�����J�ڗp�̃t���O�ύX
            if (distanceToPlayer <= visibilityDistance) // �w��͈͓��ɓ����Ă��邩
            {
                // �v���C���[��ǐ�
                agent.destination = playerTransform.position;
            }
            else
            {
                // �v���C���[�������Ȃ��A�܂��͉�������ꍇ�͏��񃍃W�b�N��
                PatrolLogic();
            }
        }
        else
        {
            // �v���C���[Transform���ݒ肳��Ă��Ȃ��ꍇ�����񃍃W�b�N��
            PatrolLogic();
            currentState = EnemyState.Walk;
        }

        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }
    }

    private void PatrolLogic()
    {
        // �ړI�n�ɓ��B�������`�F�b�N (NavMeshAgent���܂��v�Z��(pathPending)�łȂ����Ƃ��m�F)
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentState = EnemyState.Idle;
            timer += Time.deltaTime;
            //�A�j���[�V�����J�ڗp�̃t���O�ύX
            if (timer >= stayDuration)
            {
                NextDestination();
                timer = 0;
                currentState = EnemyState.Walk;
            }
        }
        else if (agent.hasPath)
        {
            // �ړI�n�Ɍ������Ă���r���̏ꍇ
            currentState = EnemyState.Walk;
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            // �ĊJ�� (isAttacking��false�ɂȂ����Ƃ�) �̂݁A�����ɖړI�n���X�V
            if (!isAttacking)
            {
                // �v���C���[�������Ă���ΒǐՁA�����Ă��Ȃ���Ώ�����ĊJ
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
