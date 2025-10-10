using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // 現在のアニメーション状態を保持 (重複呼び出し防止用)
    private EnemyMove.EnemyState currentAnimState;

    // Start関数でAnimatorコンポーネントを取得することもできます
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。アニメーションは動きません。", this);
            this.enabled = false; // アニメーションができないのでスクリプトを無効化
            return;
        }
        // 初期状態を設定
        currentAnimState = EnemyMove.EnemyState.Idle;
        SetAllBoolsFalse(); // 初期化時に全てFalseにしておくのが安全
        animator.SetBool("isIdol", true);
    }

    public void UpdateAnimation(EnemyMove.EnemyState newState)
    {
        // 状態が変わっていなければ何もしない（毎フレームSetBoolを呼ぶのを避ける）
        if (currentAnimState == newState)
        {
            return;
        }

        // 状態が変わった場合: 全てのBoolをリセット
        SetAllBoolsFalse();

        // 新しい状態に応じてBoolを設定
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

        // 状態を更新
        currentAnimState = newState;
    }

    // 全てのアニメーションBoolをfalseにするヘルパー関数
    private void SetAllBoolsFalse()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isIdol", false);
        animator.SetBool("isCloseDistanceAttack", false);
        animator.SetBool("isLongDistanceAttack", false); 
    }
}
