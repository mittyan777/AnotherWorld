using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //現在のアニメーション状態を記憶し、SetBoolの重複呼び出しを防ぐための変数
    private EnemyMove.EnemyState currentAnimState;

    //Animatorのパラメーター名
    private const string PARAM_IS_WALK = "isWalk";
    private const string PARAM_IS_IDLE = "isIdle";
    private const string PARAM_IS_LONG_ATTACK = "isLongDistanceAttack";
    private const string PARAM_IS_CLOSE_ATTACK = "isCloseDistanceAttack";

    void Start()
    {
        // Inspectorで割り当てられていない場合、子オブジェクトも含めて取得を試みる
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(gameObject.name + ": Animatorコンポーネントが見つかりません。アニメーションは動きません。", this);
            this.enabled = false;
            return;
        }

        //初期状態はIdleに設定
        currentAnimState = EnemyMove.EnemyState.Idle;
        SetAllBoolsFalse();
        animator.SetBool(PARAM_IS_IDLE, true);
    }

    public void UpdateAnimation(EnemyMove.EnemyState newState)
    {
        //状態が変わっていなければ、無駄なSetBool呼び出しを避けて終了
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
