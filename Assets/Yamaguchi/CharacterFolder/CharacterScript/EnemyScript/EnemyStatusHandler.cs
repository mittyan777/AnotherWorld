using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler : MonoBehaviour
{
    // EnemyMoveスクリプトへの参照
    private EnemyMove enemyMove;

    [SerializeField] private float stunTime = 2.0f; // スタン時間
    // 必要であれば、[SerializeField] private float knockbackForce = 5f; などもここに追加

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
        if (enemyMove == null)
        {
            Debug.LogError("EnemyMoveが見つかりません！");
            enabled = false;
        }
    }

    // Playerの魔法（スタンエリア）に当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        // タグチェック（例: PlayerMagic や StunAreaTag に変更推奨）
        if (other.gameObject.CompareTag("AttackArea01"))
        {
            // DeathやDamage状態の敵にスタンをかけないガード
            if (enemyMove.currentState != EnemyMove.EnemyState.death &&
                enemyMove.currentState != EnemyMove.EnemyState.damage)
            {
                // 既にスタン中ではないかチェック（コルーチンの重複実行を防ぐ）
                if (enemyMove.currentState != EnemyMove.EnemyState.Stun)
                {
                    StartCoroutine(StunSequence());
                }
            }
        }
    }

    // スタン処理のコルーチン
    private IEnumerator StunSequence()
    {
        // 1. スタン開始の指示
        enemyMove.SetStunState(true);

        // 2. スタン待機時間
        yield return new WaitForSeconds(stunTime);

        // 3. スタン解除の指示
        // ただし、スタン中に死亡した場合は処理をスキップ
        if (enemyMove.currentState != EnemyMove.EnemyState.death)
        {
            enemyMove.SetStunState(false);
        }
    }
}
