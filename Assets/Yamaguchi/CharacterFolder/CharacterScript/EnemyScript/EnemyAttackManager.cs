using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviourを継承させることで、子クラスもコルーチンを実行可能になる
public abstract class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2.0f; // 攻撃できる距離
    [SerializeField] protected float attackInterval = 1.5f; // 攻撃のクールタイム
    [SerializeField] protected float animationlatency = 0.25f;//アニメーション再生までの待ち時間
    [SerializeField] protected float animationRecoveryTime = 0.75f;//アニメーションが終わるまでの時間

    protected Transform playerTransform;
    protected EnemyMove enemyMovement;

    protected float attackTimer;
    protected bool isPlayerInRange = false;
    protected bool isAttackSequenceRunning = false;

    //抽象メソッド：子クラスで具体的な攻撃ロジックを実装させる
    //このコルーチンは攻撃判定の実行部分のみを担当する
    protected abstract IEnumerator PerformAttackLogic();
    //子クラスに実装させる：アニメーションの状態設定 
    protected abstract void SetAttackAnimation();
    protected abstract void ResetAttackAnimation();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        enemyMovement = GetComponentInParent<EnemyMove>();
        if (enemyMovement == null)
        {
            Debug.LogError(gameObject.name + ": EnemyMoveが見つかりません。", this);
            this.enabled = false;
            return;
        }

        attackTimer = attackInterval;

        //子クラスの初期化メソッドを呼び出す
        OnInit();
    }

    protected virtual void OnInit() { }

    void Update()
    {
        if (!this.enabled) return;

        if (playerTransform == null || enemyMovement == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //攻撃範囲内かどうかを判定
        isPlayerInRange = distanceToPlayer <= attackRange;

        //攻撃実行ロジック
        if (isPlayerInRange && !isAttackSequenceRunning)
        {
            AttackCheck();
        }

        //敵の向きをプレイヤーに合わせる（攻撃待機/実行中のみ）
        if (isPlayerInRange)
        {
            RotateTowardsPlayer();
        }
    }

    private void AttackCheck()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            StartCoroutine(AttackSequence());
            attackTimer = 0f;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    //攻撃の実行・待機・移動制御をラップするコルーチン
    protected IEnumerator AttackSequence()
    {
        /*
        isAttackSequenceRunning = true;

        //移動を停止させる
        enemyMovement.SetIsStoppedByAttack(true);
        
        //子クラスを通じて、この攻撃のアニメーション状態を設定
        SetAttackAnimation();
        
        //攻撃アニメーションの開始まで少し待つ
        yield return new WaitForSeconds(animationlatency);

        //PerformAttackLogicが終了するのを待つ
        yield return StartCoroutine(PerformAttackLogic());

        //アニメーションのリカバリー時間待機
        yield return new WaitForSeconds(animationRecoveryTime); //共通の硬直時間

        //攻撃終了+移動を再開させる
        ResetAttackAnimation();
        enemyMovement.SetIsStoppedByAttack(false);

        isAttackSequenceRunning = false;
        */
        isAttackSequenceRunning = true;

        //移動を停止させる
        enemyMovement.SetIsStoppedByAttack(true);

        //子クラスを通じて、この攻撃のアニメーション状態を設定
        SetAttackAnimation();

        if (enemyMovement != null)
        {
            enemyMovement.ForceUpdateAnimation();
        }

        //PerformAttackLogicが終了するのを待つ
        yield return StartCoroutine(PerformAttackLogic());

        //攻撃判定が終わったら、すぐにIdle状態に戻す命令を出す
        enemyMovement.currentState = EnemyMove.EnemyState.Idle;

        //isStoppedByAttackは、ゲームデザイン上の硬直がなければ、ここで false にする
        enemyMovement.SetIsStoppedByAttack(false); 

        ResetAttackAnimation();

        isAttackSequenceRunning = false;

        yield return null;
    }
}