using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // ���݂̃A�j���[�V������Ԃ�ێ� (�d���Ăяo���h�~�p)
    private EnemyMove.EnemyState currentAnimState;

    // Start�֐���Animator�R���|�[�l���g���擾���邱�Ƃ��ł��܂�
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animator�R���|�[�l���g��������܂���B�A�j���[�V�����͓����܂���B", this);
            this.enabled = false; // �A�j���[�V�������ł��Ȃ��̂ŃX�N���v�g�𖳌���
            return;
        }
        // ������Ԃ�ݒ�
        currentAnimState = EnemyMove.EnemyState.Idle;
        SetAllBoolsFalse(); // ���������ɑS��False�ɂ��Ă����̂����S
        animator.SetBool("isIdol", true);
    }

    public void UpdateAnimation(EnemyMove.EnemyState newState)
    {
        // ��Ԃ��ς���Ă��Ȃ���Ή������Ȃ��i���t���[��SetBool���ĂԂ̂������j
        if (currentAnimState == newState)
        {
            return;
        }

        // ��Ԃ��ς�����ꍇ: �S�Ă�Bool�����Z�b�g
        SetAllBoolsFalse();

        // �V������Ԃɉ�����Bool��ݒ�
        switch (newState)
        {
            case EnemyMove.EnemyState.Idle:
                animator.SetBool("isIdol", true);
                break;
            case EnemyMove.EnemyState.Walk:
                animator.SetBool("isWalk", true);
                break;
            case EnemyMove.EnemyState.Attack:
                animator.SetBool("isCloseDistanceAttack", true);
                break;
        }

        // ��Ԃ��X�V
        currentAnimState = newState;
    }

    // �S�ẴA�j���[�V����Bool��false�ɂ���w���p�[�֐�
    private void SetAllBoolsFalse()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isIdol", false);
        animator.SetBool("isCloseDistanceAttack", false);
        animator.SetBool("isLongDistanceAttack", false); 
    }
}
