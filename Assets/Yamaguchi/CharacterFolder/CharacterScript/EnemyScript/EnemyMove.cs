using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // NavMesh Agent
    private NavMeshAgent agent;

    // ================ ����ϐ� ================
    [SerializeField] private float stayDuration = 3f; // �ҋ@����
    private float timer;

    // ================ �ǐՁE��ԕϐ� ================
    [SerializeField] private float visibilityDistance = 15f; // �v���C���[���F����
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float stopDistance = 1.5f; // �v���C���[�����~����������

    public enum EnemyState { Idle, Walk, CloseAttack, RangedAttack, None }
    public EnemyState currentState = EnemyState.Idle;

    [SerializeField] EnemyAnimationManager animationManager;

    // �U���ɂ���Ĉړ�����~����Ă�����
    private bool isStoppedByAttack = false;

    // =========================================================

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // EnemyAnimationManager�̎擾
        animationManager = GetComponent<EnemyAnimationManager>();
        if (animationManager == null) animationManager = GetComponentInChildren<EnemyAnimationManager>();
        if (animationManager == null)
        {
            Debug.LogError(gameObject.name + ": EnemyAnimationManager���A�^�b�`����Ă��܂���B�A�j���[�V�����͓����܂���B", this);
        }

        if (agent == null)
        {
            Debug.LogError(gameObject.name + ": NavMeshAgent���A�^�b�`����Ă��܂���B", this);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Agent�ɂ���]������蓮�ɂ���
        if (agent != null) agent.updateRotation = false;

        // �ŏ��̖ړI�n��ݒ�
        NextDestination();
    }

    private void Update()
    {
        // �U���ȂǂŒ�~���̏ꍇ�A�ړ����W�b�N���X�L�b�v
        if (isStoppedByAttack)
        {
            // �U�����͉�]���ړ������Ȃ��̂ŁA�����ŏI��
            if (animationManager != null) animationManager.UpdateAnimation(currentState);
            return;
        }

        // �ǐՁE���񃍃W�b�N
        UpdateMovementState();

        // �����̍X�V (�ړ����Ă���ꍇ�̂�)
        UpdateRotation();

        // �A�j���[�V�����X�V
        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }
    }

    // ================ �ړ��E��ԃ��W�b�N ================

    private void UpdateMovementState()
    {
        // �v���C���[�̎��F�ƒǐՃ��W�b�N
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= visibilityDistance)
            {
                // �ǐՋ������ɓ������ꍇ
                if (distanceToPlayer > stopDistance)
                {
                    // �ǐ�: �v���C���[����stopDistance�����ꂽ�n�_��ړI�n�ɂ���
                    Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    Vector3 targetPosition = playerTransform.position - directionToPlayer * stopDistance;

                    agent.destination = targetPosition;
                    currentState = EnemyState.Walk;
                }
                else
                {
                    // ��~�ڕW�����ɓ��B�����ꍇ
                    currentState = EnemyState.Idle; // �U���͈͓��Ȃ�Idle�őҋ@
                }
                return;
            }
        }

        // �v���C���[�������Ȃ��A�܂��͂��Ȃ��ꍇ�͏��񃍃W�b�N��
        PatrolLogic();
    }

    private void PatrolLogic()
    {
        if (agent == null || !agent.enabled) return;

        // �ړI�n�ɓ��B�������`�F�b�N (remainingDistance < stopDistance���g�p)
        if (agent.remainingDistance < stopDistance && !agent.pathPending)
        {
            // ����������Idle�őҋ@
            currentState = EnemyState.Idle;
            timer += Time.deltaTime;

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

    // ================ Helper �֐� ================

    private void NextDestination()
    {
        if (agent != null && agent.enabled && !agent.isStopped)
        {
            NavMeshHit hit;
            // NavMesh��̗L���ȏꏊ��T��
            if (NavMesh.SamplePosition(
                    new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20)),
                    out hit,
                    5.0f,
                    NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
            else
            {
                Debug.LogWarning("NavMesh��ɗL���ȃ����_���ړI�n��������܂���ł����B");
            }
        }
    }

    private void UpdateRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.01f) // �ړ����Ă��邩�m�F
        {
            // �ڕW�̉�] (�ړ������� Quaternion ���쐬)
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);

            // �X���[�Y�ɕ��
            float rotationSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // ================ �O������̃C���^�[�t�F�[�X ================

    public void SetIsStoppedByAttack(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            // �U���I���� (isAttacking��false�ɂȂ����Ƃ�) �̂݁A�ړI�n���Đݒ�
            if (!isAttacking)
            {
                // �ړ����W�b�N�ɖ߂�AUpdateMovementState()�Ŏ����I�ɒǐ� or ���񂪍ĊJ�����
                // ������destination���Đݒ肷��K�v�͂Ȃ�
            }
        }
    }
}