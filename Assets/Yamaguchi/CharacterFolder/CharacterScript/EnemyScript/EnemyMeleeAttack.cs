using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttackManager
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float hitboxDuration = 0.5f;

    protected override void OnInit()
    {
        // 攻撃オブジェクトは初期状態で無効にしておく
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    protected override IEnumerator PerformAttack()
    {
        //攻撃オブジェクトを有効にする (攻撃判定発生)
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        //攻撃判定を一定時間維持 (0.5秒)
        yield return new WaitForSeconds(hitboxDuration);

        //攻撃オブジェクトを無効にする (攻撃判定終了)
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }
}
