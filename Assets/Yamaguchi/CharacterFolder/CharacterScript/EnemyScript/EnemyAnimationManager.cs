using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //���݂̃A�j���[�V������Ԃ��L�����ASetBool�̏d���Ăяo����h�����߂̕ϐ�
    private EnemyMove.EnemyState currentAnimState;

    //Animator�̃p�����[�^�[��
    private const string PARAM_IS_WALK = "isWalk";
    private const string PARAM_IS_IDLE = "isIdle";
    private const string PARAM_IS_LONG_ATTACK = "isLongDistanceAttack";
    private const string PARAM_IS_CLOSE_ATTACK = "isCloseDistanceAttack";

    void Start()
    {
        // Inspector�Ŋ��蓖�Ă��Ă��Ȃ��ꍇ�A�q�I�u�W�F�N�g���܂߂Ď擾�����݂�
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animator�R���|�[�l���g��������܂���B�A�j���[�V�����͓����܂���B", this);
            this.enabled = false;
            return;
        }

        //������Ԃ�Idle�ɐݒ�
        currentAnimState = EnemyMove.EnemyState.Idle;
        SetAllBoolsFalse();
        animator.SetBool(PARAM_IS_IDLE, true);
    }

    public void UpdateAnimation(EnemyMove.EnemyState newState)
    {
        //��Ԃ��ς���Ă��Ȃ���΁A���ʂ�SetBool�Ăяo��������ďI��
        if (currentAnimState == newState)
        {
            return;
        }

        SetAllBoolsFalse();

        switch (newState)
        {
            case EnemyMove.EnemyState.Idle:
                animator.SetBool(PARAM_IS_IDLE, true);
                break;
            case EnemyMove.EnemyState.Walk:
                animator.SetBool(PARAM_IS_WALK, true);
                break;
            case EnemyMove.EnemyState.CloseAttack:
                animator.SetBool(PARAM_IS_CLOSE_ATTACK, true);
                break;
            case EnemyMove.EnemyState.RangedAttack:
                animator.SetBool(PARAM_IS_LONG_ATTACK, true);
                break;
            case EnemyMove.EnemyState.None:
                break;
        }

        currentAnimState = newState;
    }

    private void SetAllBoolsFalse()
    {
        if (animator == null) return;

        animator.SetBool(PARAM_IS_WALK, false);
        animator.SetBool(PARAM_IS_IDLE, false);
        animator.SetBool(PARAM_IS_CLOSE_ATTACK, false);
        animator.SetBool(PARAM_IS_LONG_ATTACK, false);
    }
}
