using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttackManager
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float hitboxDuration; //攻撃判定の持続時間
    [SerializeField]private float displaylatency;//攻撃の当たり判定の表示待ち時間
    protected override void OnInit()
    {
        // 攻撃オブジェクトは初期状態で無効にしておく
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    // 攻撃判定の発生と終了のみを制御
    protected override IEnumerator PerformAttackLogic()
    {
        // アニメーションの振りかぶり時間に合わせて少し待つ (例: 0.15秒)
        yield return new WaitForSeconds(displaylatency);
        // 攻撃判定を有効にする
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        // 攻撃判定を一定時間維持
        yield return new WaitForSeconds(hitboxDuration);

        // 攻撃判定を無効にする
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    //近接攻撃のアニメーション状態を設定
    protected override void SetAttackAnimation()
    {
        if (enemyMovement != null)
        {
            enemyMovement.currentState = EnemyMove.EnemyState.CloseAttack;
        }
    }

    //アニメーション状態をリセット
    protected override void ResetAttackAnimation()
    {
        // 攻撃終了時にアニメーション状態をリセット（Idleに戻すのはAttackSequenceが行うため、ここでは何もしなくて良い）
    }
}