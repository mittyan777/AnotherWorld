using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] protected float engageRange = 5f;
    [SerializeField] protected float attackRate = 1.5f;
    protected Transform playerTransform;
    protected EnemyMove enemyMovement;
    protected float attackTimer;
    protected bool isPlayerInRange = false;
    protected bool isAttacking = false; 

    // ���ۃ��\�b�h�F�q�N���X�ŋ�̓I�ȍU�����W�b�N������������
    protected abstract IEnumerator PerformAttack();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        enemyMovement = GetComponent<EnemyMove>();

        attackTimer = attackRate;

        // �q�N���X�̏��������\�b�h���Ăяo��
        OnInit();
    }

    protected virtual void OnInit() { }

    void Update()
    {
        if (playerTransform == null || enemyMovement == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool wasPlayerInRange = isPlayerInRange;
        isPlayerInRange = distanceToPlayer <= engageRange;

        // �͈͓��ɓ��������A�O�ɏo�����𔻒�
        if (isPlayerInRange && !wasPlayerInRange)
        {
            // �͈͓��ɓ������u��: �ړ����~
            enemyMovement.SetIsAttacking(true);
        }
        else if (!isPlayerInRange && wasPlayerInRange)
        {
            // �͈͊O�ɏo���u��: �ړ����ĊJ
            enemyMovement.SetIsAttacking(false);
        }

        // �U�����s���W�b�N
        if (isPlayerInRange && !isAttacking)
        {
            AttackCheck();
        }
    }

    private void AttackCheck()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            StartCoroutine(AttackSequence());
            attackTimer = 0f;
        }
    }

    // �U���̎��s�E�ҋ@�E�ړ���������b�v����R���[�`��
    IEnumerator AttackSequence()
    {
        isAttacking = true;

        // �q�N���X�̋�̓I�ȍU�����W�b�N�����s
        yield return StartCoroutine(PerformAttack());

        isAttacking = false;
    }
}
