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

    // 抽象メソッド：子クラスで具体的な攻撃ロジックを実装させる
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

        // 子クラスの初期化メソッドを呼び出す
        OnInit();
    }

    protected virtual void OnInit() { }

    void Update()
    {
        if (playerTransform == null || enemyMovement == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool wasPlayerInRange = isPlayerInRange;
        isPlayerInRange = distanceToPlayer <= engageRange;

        // 範囲内に入ったか、外に出たかを判定
        if (isPlayerInRange && !wasPlayerInRange)
        {
            // 範囲内に入った瞬間: 移動を停止
            enemyMovement.SetIsAttacking(true);
        }
        else if (!isPlayerInRange && wasPlayerInRange)
        {
            // 範囲外に出た瞬間: 移動を再開
            enemyMovement.SetIsAttacking(false);
        }

        // 攻撃実行ロジック
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

    // 攻撃の実行・待機・移動制御をラップするコルーチン
    IEnumerator AttackSequence()
    {
        isAttacking = true;

        // 子クラスの具体的な攻撃ロジックを実行
        yield return StartCoroutine(PerformAttack());

        isAttacking = false;
    }
}
