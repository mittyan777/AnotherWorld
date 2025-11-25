using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //現在のアニメーション状態を記憶し、SetBoolの重複呼び出しを防ぐための変数
    private BossMove01.Boss01ActionType boss01Animation;
    private BossMove02.Boss02ActionType boss02Animation;

    //Animatorのパラメーター名
    private const string PARAM_IS_WALK = "IsWalk";
    private const string PARAM_IS_IDLE = "IsIdle";
    private const string PARAM_IS_ATTACK01 = "IsAttack01";
    private const string PARAM_IS_ATTACK02 = "IsAttack02";
    private const string PARAM_IS_ATTACK03 = "IsAttack03";
    private const string PARAM_IS_Death = "IsDeath";

    void Start()
    {
        // Inspectorで割り当てられていない場合、子オブジェクトも含めて取得を試みる
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            this.enabled = false;
            return;
        }

        //初期状態はIdleに設定
        boss01Animation = BossMove01.Boss01ActionType.Idle;
        SetAllBoolsFalse();
        animator.SetBool(PARAM_IS_IDLE, true);

        boss02Animation=BossMove02.Boss02ActionType.Idle;
        SetAllBoolsFalse();
        animator.SetBool(PARAM_IS_IDLE, true);
    }

    public void Boss01UpdateAnimation(BossMove01.Boss01ActionType boss01State)
    {
        //状態が変わっていなければ、無駄なSetBool呼び出しを避けて終了
        if (boss01State != BossMove01.Boss01ActionType.Death)
        {
            // 状態が変わっていなければ、無駄なSetBool呼び出しを避けて終了
            if (boss01Animation == boss01State)
            {
                return;
            }
        }

        SetAllBoolsFalse();

        switch (boss01State)
        {
            case BossMove01.Boss01ActionType.Idle:
                animator.SetBool(PARAM_IS_IDLE, true);
                break;
            case BossMove01.Boss01ActionType.Walking:
                animator.SetBool(PARAM_IS_WALK, true);
                break;
            case BossMove01.Boss01ActionType.QuickAttack:
                animator.SetBool(PARAM_IS_ATTACK01, true);
                break;
            case BossMove01.Boss01ActionType.Tackle:
                animator.SetBool(PARAM_IS_ATTACK02, true);
                break;
            case BossMove01.Boss01ActionType.StrongAttack:
                animator.SetBool(PARAM_IS_ATTACK03, true);
                break;
            case BossMove01.Boss01ActionType.Death:
                animator.SetBool(PARAM_IS_Death, true);
                break;
            case BossMove01.Boss01ActionType.None:
                break;
        }

        boss01Animation = boss01State;
    }

    public void Boss02UpdateAnimation(BossMove02.Boss02ActionType boss02State)
    {
        //状態が変わっていなければ、無駄なSetBool呼び出しを避けて終了
        if (boss02State != BossMove02.Boss02ActionType.Death)
        {
            // 状態が変わっていなければ、無駄なSetBool呼び出しを避けて終了
            if (boss02Animation == boss02State)
            {
                return;
            }
        }

        SetAllBoolsFalse();

        switch (boss02State)
        {
            case BossMove02.Boss02ActionType.Idle:
                animator.SetBool(PARAM_IS_IDLE, true);
                break;
            case BossMove02.Boss02ActionType.Walking:
                animator.SetBool(PARAM_IS_WALK, true);
                break;
            case BossMove02.Boss02ActionType.QuickAttack:
                animator.SetBool(PARAM_IS_ATTACK01, true);
                break;
            case BossMove02.Boss02ActionType.StrongAttack:
                animator.SetBool(PARAM_IS_ATTACK02, true);
                break;
            case BossMove02.Boss02ActionType.TwoStepAttack:
                animator.SetBool(PARAM_IS_ATTACK03, true);
                break;
            case BossMove02.Boss02ActionType.Death:
                animator.SetBool(PARAM_IS_Death, true);
                break;
            case BossMove02.Boss02ActionType.None:
                break;
        }

        boss02Animation = boss02State;
    }

    private void SetAllBoolsFalse()
    {
        if (animator == null) return;

        animator.SetBool(PARAM_IS_WALK, false);
        animator.SetBool(PARAM_IS_IDLE, false);
        animator.SetBool(PARAM_IS_ATTACK01, false);
        animator.SetBool(PARAM_IS_ATTACK02, false);
        animator.SetBool(PARAM_IS_ATTACK03, false);
        animator.SetBool(PARAM_IS_Death, false);
    }
}
