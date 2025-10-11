using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour���p�������邱�ƂŁA�q�N���X���R���[�`�������s�\�ɂȂ�
public abstract class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2.0f; // �U���ł��鋗��
    [SerializeField] protected float attackInterval = 1.5f; // �U���̃N�[���^�C��

    protected Transform playerTransform;
    protected EnemyMove enemyMovement;

    protected float attackTimer;
    protected bool isPlayerInRange = false;
    protected bool isAttackSequenceRunning = false; // �U���R���[�`�������s����

    // ���ۃ��\�b�h�F�q�N���X�ŋ�̓I�ȍU�����W�b�N������������ (�q�b�g�{�b�N�XON/OFF�Ȃ�)
    // ���̃R���[�`���͍U������̎��s�����݂̂�S������
    protected abstract IEnumerator PerformAttackLogic();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        enemyMovement = GetComponentInParent<EnemyMove>(); // �e������擾�ł���悤�C��
        if (enemyMovement == null)
        {
            Debug.LogError(gameObject.name + ": EnemyMove��������܂���B", this);
            this.enabled = false;
            return;
        }

        attackTimer = attackInterval;

        // �q�N���X�̏��������\�b�h���Ăяo��
        OnInit();
    }

    protected virtual void OnInit() { }

    void Update()
    {
        if (playerTransform == null || enemyMovement == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // �U���͈͓����ǂ����𔻒� (������ stopDistance �� attackRange �̊Ԃł���ׂ�)
        // enemyMovement.stopDistance < distanceToPlayer < attackRange �͈̔͂ōU��
        isPlayerInRange = distanceToPlayer <= attackRange;

        // �U�����s���W�b�N
        if (isPlayerInRange && !isAttackSequenceRunning)
        {
            AttackCheck();
        }

        // �G�̌������v���C���[�ɍ��킹��i�U���ҋ@/���s���̂݁j
        if (isPlayerInRange)
        {
            RotateTowardsPlayer();
        }
    }

    private void AttackCheck()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            StartCoroutine(AttackSequence());
            attackTimer = 0f;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0; // Y����]�݂̂ɐ���
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // �U���̎��s�E�ҋ@�E�ړ���������b�v����R���[�`�� (�S�Ă������Ő���)
    protected IEnumerator AttackSequence()
    {
        isAttackSequenceRunning = true;

        // 1. �ړ����~������
        enemyMovement.SetIsStoppedByAttack(true);

        // 2. �U���A�j���[�V�����̊J�n�܂ŏ����҂� (��: �U���J�n���[�V�����̔���)
        yield return new WaitForSeconds(0.1f);

        // 3. �q�N���X�̋�̓I�ȍU�����W�b�N�i�q�b�g�{�b�N�XON/OFF�A�e�����Ȃǁj�����s
        // ������PerformAttackLogic���I������̂�҂�
        yield return StartCoroutine(PerformAttackLogic());

        // 4. �A�j���[�V�����̃��J�o���[���ԑҋ@
        yield return new WaitForSeconds(0.5f); // ���ʂ̍d������

        // 5. �U���I��: �ړ����ĊJ������
        enemyMovement.SetIsStoppedByAttack(false);

        isAttackSequenceRunning = false;
    }
}