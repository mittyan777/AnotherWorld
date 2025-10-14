using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // NavMesh Agent
    private NavMeshAgent agent;

    // ================ 巡回変数 ================
    [SerializeField] private float stayDuration = 3f; // 待機時間
    private float timer;

    // ================ 追跡・状態変数 ================
    [SerializeField] private float visibilityDistance = 15f; // プレイヤー視認距離
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float stopDistance = 1.5f; // プレイヤーから停止したい距離

    public enum EnemyState { Idle, Walk, CloseAttack, RangedAttack, None }
    public EnemyState currentState = EnemyState.Idle;

    [SerializeField] EnemyAnimationManager animationManager;

    // 攻撃によって移動が停止されている状態
    private bool isStoppedByAttack = false;

    // =========================================================

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // EnemyAnimationManagerの取得
        animationManager = GetComponent<EnemyAnimationManager>();
        if (animationManager == null) animationManager = GetComponentInChildren<EnemyAnimationManager>();
        if (animationManager == null)
        {
            Debug.LogError(gameObject.name + ": EnemyAnimationManagerがアタッチされていません。アニメーションは動きません。", this);
        }

        if (agent == null)
        {
            Debug.LogError(gameObject.name + ": NavMeshAgentがアタッチされていません。", this);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Agentによる回転制御を手動にする
        if (agent != null) agent.updateRotation = false;

        // 最初の目的地を設定
        NextDestination();
    }

    private void Update()
    {
        // 攻撃などで停止中の場合、移動ロジックをスキップ
        if (isStoppedByAttack)
        {
            // 攻撃中は回転も移動もしないので、ここで終了
            if (animationManager != null) animationManager.UpdateAnimation(currentState);
            return;
        }

        // 追跡・巡回ロジック
        UpdateMovementState();

        //向きの更新 (移動している場合のみ)
        UpdateRotation();

        // アニメーション更新
        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }
    }

    // ================ 移動・状態ロジック ================

    private void UpdateMovementState()
    {
        // プレイヤーの視認と追跡ロジック
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= visibilityDistance)
            {
                // 追跡距離内に入った場合
                if (distanceToPlayer > stopDistance)
                {
                    // 追跡:プレイヤーからstopDistance分離れた地点を目的地にする
                    Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    Vector3 targetPosition = playerTransform.position - directionToPlayer * stopDistance;

                    agent.destination = targetPosition;
                    currentState = EnemyState.Walk;
                }
                else
                {
                    // 停止目標距離に到達した場合
                    currentState = EnemyState.Idle; // 攻撃範囲内ならIdleで待機
                }
                return;
            }
        }
        // プレイヤーが見えない、またはいない場合は巡回ロジックへ
        PatrolLogic();
    }

    private void PatrolLogic()
    {
        /*
        currentState=EnemyState.Walk;
        if (agent == null || !agent.enabled) return;

        // 目的地に到達したかチェック (remainingDistance < stopDistanceを使用)
        if (agent.remainingDistance < stopDistance && 
            !agent.pathPending)
        {
            // 到着したらIdleで待機
            currentState = EnemyState.Idle;
            timer += Time.deltaTime;

            if (timer >= stayDuration)
            {
                NextDestination();
                timer = 0;
                currentState = EnemyState.Walk;
            }
        }
        else if (agent.hasPath)
        {
            // 目的地に向かっている途中の場合
            currentState = EnemyState.Walk;
        }
        */
        if (agent == null || !agent.enabled) return;

        // PatrolLogicの先頭で currentStateを Walkに強制的に設定するのは削除
        // currentState=EnemyState.Walk; // ← これを削除

        // 目的地に到達したかの判定。非常に小さな閾値を使う (例: 0.5f)
        float arrivalThreshold = 0.5f;

        bool destinationReached =
            agent.hasPath &&
            agent.remainingDistance <= arrivalThreshold && //到達閾値を厳しくする
            !agent.pathPending;

        if (destinationReached)
        {
            // 目的地に到達した場合

            // まだIdleでない場合、Idleに切り替え、タイマーをリセット
            if (currentState != EnemyState.Idle)
            {
                currentState = EnemyState.Idle;
                timer = 0f;
                // 到達したらNavMeshAgentを停止させる（アニメーションがIdleになったことを保証）
                agent.isStopped = true;
            }

            // Idle状態の場合のみタイマーを進める
            if (currentState == EnemyState.Idle)
            {
                timer += Time.deltaTime;

                if (timer >= stayDuration)
                {
                    agent.isStopped = false; // 移動を再開してからNextDestinationを呼ぶ
                    NextDestination();
                    timer = 0;
                    currentState = EnemyState.Walk;
                }
            }
        }
        else // 目的地に向かっている途中、または新しい目的地を設定した直後
        {
            if (agent.isStopped) agent.isStopped = false; // 停止状態から復帰

            // 有効なパスがある場合、Walk状態にする
            if (agent.hasPath || (agent.destination - transform.position).sqrMagnitude > arrivalThreshold * arrivalThreshold)
            {
                currentState = EnemyState.Walk;
            }
        }
    }

    // ================ Helper 関数 ================

    private void NextDestination()
    {
        if (agent != null && agent.enabled && !agent.isStopped)
        {
            NavMeshHit hit;
            // NavMesh上の有効な場所を探す
            if (NavMesh.SamplePosition(
                    new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20)),
                    out hit,
                    5.0f,
                    NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
            else
            {
                Debug.LogWarning("NavMesh上に有効なランダム目的地が見つかりませんでした。");
            }
        }
    }

    private void UpdateRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.01f) //移動しているか確認
        {
            // 目標の回転 (移動方向に Quaternion を作成)
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);

            // スムーズに補間
            float rotationSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // ================ 外部からのインターフェース ================
    public void ForceUpdateAnimation()
    {
        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }
    }

    public void SetIsStoppedByAttack(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            //攻撃終了時
            if (!isAttacking)
            {
                //Playerが視認範囲外にいる場合、現在の目的地を無効化し、巡回ロジックで新しいランダムな目的地を強制的に取得させる
                if (playerTransform == null || Vector3.Distance(transform.position, playerTransform.position) > visibilityDistance)
                {
                    //現在の目的地を無効化（新しいパスを強制的に計算させる）
                    agent.ResetPath();
                    NextDestination(); 
                }
            }
        }
    }
}