using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttackManager
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float hitboxDuration = 0.3f; // 攻撃判定の持続時間

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
}