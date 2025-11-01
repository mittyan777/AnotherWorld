using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public enum EnemyState { 
        Idle,
        Walk, 
        CloseAttack,
        RangedAttack,
        damage,
        death,
        None 
    }

    [HideInInspector] public EnemyState currentState = EnemyState.Idle; // 外部（攻撃スクリプト）から変更されることを考慮し、HideInInspector

    [SerializeField] EnemyAnimationManager animationManager;

    // NavMeshAgentの到達判定に使用する小さな閾値
    private const float ARRIVAL_THRESHOLD = 0.5f;


    [SerializeField] private float stayDuration = 3f;           //目的地到着後の待機時間 (秒)
    [SerializeField] private float patrolRandomRange = 10f;     //ランダムな目的地を探す自分の位置からの半径

    [SerializeField] private float visibilityDistance = 15f;    //プレイヤー視認距離
    [SerializeField] private float stopDistance = 1.5f;         //プレイヤーから離れて停止したい距離 (追跡時)
    //
    private Transform playerTransform;
    private float patrolTimer;                                  //巡回待機用のタイマー
    private bool isStoppedByAttack = false;                     //攻撃スクリプトによって移動が停止されているか

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 必須コンポーネントとPlayerの取得
        InitializeComponents();

        // NavMeshAgentの回転をコードで制御
        if (agent != null) agent.updateRotation = false;

        // 最初の巡回目的地を設定し、移動を開始
        SetRandomDestination();
    }

    private void Update()
    {
        EnemyState previousState = currentState;
        if (currentState == EnemyState.death)
        {
            return;
        }
        //ダメージ中、もしくは攻撃中なら移動などの処理をスキップする
        if (currentState == EnemyState.damage || isStoppedByAttack)
        {
            //アニメーションは更新する
            UpdateAnimationState(true);
            return;
        }

        UpdateMovementState();
        UpdateRotation();
        UpdateAnimationState(false);
    }

    private void InitializeComponents()
    {
        // EnemyAnimationManagerの取得
        animationManager = GetComponent<EnemyAnimationManager>();
        if (animationManager == null) animationManager = GetComponentInChildren<EnemyAnimationManager>();
        if (animationManager == null)
        {
            Debug.LogError(gameObject.name + ": EnemyAnimationManagerがアタッチされていません。", this);
        }

        if (agent == null)
        {
            Debug.LogError(gameObject.name + ": NavMeshAgentがアタッチされていません。", this);
            this.enabled = false;
            return;
        }

        // PlayerTransformの取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void UpdateMovementState()
    {
        // プレイヤーが視認範囲内にいるかチェック
        bool playerVisible = CheckPlayerVisibility();

        if (playerVisible)
        {
            ChasePlayer();
        }
        else
        {
            PatrolLogic();
        }
    }

    private bool CheckPlayerVisibility()
    {
        if (playerTransform == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= visibilityDistance;
    }

    private void PatrolLogic()
    {
        if (agent == null || !agent.enabled) return;

        //目的地に到達したかの判定
        bool destinationReached =
            agent.hasPath &&
            agent.remainingDistance <= ARRIVAL_THRESHOLD &&
            !agent.pathPending;

        //停止中ではないが、目的地もない（初期状態やパス失敗）
        bool noPathOrStationary = !agent.hasPath && !agent.pathPending && !agent.isStopped;

        if (destinationReached || noPathOrStationary)
        {
            // 目的地に到達した、または移動が完了した/失敗したと判断した場合
            if (currentState != EnemyState.Idle)
            {
                currentState = EnemyState.Idle;
                patrolTimer = 0f;
                agent.isStopped = true; // 確実に停止
            }
        }

        // Idle状態 (待機中) のロジック
        if (currentState == EnemyState.Idle)
        {
            if (CheckPlayerVisibility())
            {
                // Idle状態を即座に終了し、次のUpdateでChaseに移行
                agent.isStopped = false; // Chaseのために移動を再開
                return;
            }

            patrolTimer += Time.deltaTime;

            if (patrolTimer >= stayDuration)
            {
                //移動再開、新しい目的地を設定、状態をWalkに
                agent.isStopped = false;
                SetRandomDestination();
                currentState = EnemyState.Walk;
                patrolTimer = 0;
            }
        }
        else if (currentState == EnemyState.Walk)
        {
            if (!agent.hasPath && !agent.pathPending)
            {
                // 強制的に目的地を再設定
                SetRandomDestination();
            }
            // 移動中に停止させられていたら解除
            if (agent.isStopped) agent.isStopped = false;
        }
    }

    // ランダムな巡回目的地を設定する
    private void SetRandomDestination()
    {
        if (agent == null || !agent.enabled) return;

        // 自分の位置を中心とした patrolRandomRange の範囲内で目的地を探す
        Vector3 randomDirection = Random.insideUnitSphere * patrolRandomRange;
        randomDirection += transform.position;

        NavMeshHit hit;

        // NavMesh上の有効な場所を探す (探索半径も patrolRandomRange を使用)
        if (NavMesh.SamplePosition(
                randomDirection,
                out hit,
                patrolRandomRange,
                NavMesh.AllAreas))
        {
            agent.destination = hit.position;
            currentState = EnemyState.Walk;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": NavMesh上に有効なランダム目的地が見つかりませんでした。再試行します。", this);
            // 目的地が見つからない場合、すぐにIdleに戻り、次のフレームでPatrolLogicが再試行する
            currentState = EnemyState.Idle;
            patrolTimer = 0;
            agent.isStopped = true;
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // NavMeshAgentの移動を再開
        if (agent.isStopped) agent.isStopped = false;

        if (distanceToPlayer > stopDistance)
        {
            //プレイヤーから指定距離離れた地点を目的地にする
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 targetPosition = playerTransform.position - directionToPlayer * stopDistance;

            agent.destination = targetPosition;
            currentState = EnemyState.Walk;
        }
        else
        {
            // 停止目標距離に到達した場合（攻撃範囲内）
            agent.isStopped = true;
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateRotation()
    {
        //速度がある（移動中）の場合のみ回転
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            float rotationSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    //アニメーションマネージャーの状態を更新
    private void UpdateAnimationState(bool isAttackOverride)
    {
        if (animationManager == null) return;

        //ダメージと死は何よりも優先されるアニメーション
        if (currentState == EnemyState.damage || 
            currentState == EnemyState.death)
        {
            animationManager.UpdateAnimation(currentState);
            return;
        }

        EnemyState finalState = EnemyState.Idle;

        //攻撃による上書き
        if (isAttackOverride || 
            currentState == EnemyState.CloseAttack || 
            currentState == EnemyState.RangedAttack)
        {
            finalState = currentState;
        }
        //移動状態の判定
        else if (currentState == EnemyState.Walk)
        {
            // Agentが移動中であることを確認（NavMeshAgentの速度チェック）
            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                finalState = EnemyState.Walk;
            }
            else
            {
                // Walk状態だが動いていない場合はIdleに設定
                finalState = EnemyState.Idle;
            }
        }
        else
        {
            finalState = EnemyState.Idle;
        }

        animationManager.UpdateAnimation(finalState);
    }

    // 攻撃スクリプトから呼び出され、移動を一時停止・再開させる
    public void SetIsStoppedByAttack(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            // 攻撃終了時
            if (!isAttacking)
            {
                // Playerが視認範囲外にいる場合、巡回に戻る
                if (playerTransform == null || Vector3.Distance(transform.position, playerTransform.position) > visibilityDistance)
                {
                    agent.ResetPath(); // 現在の目的地を無効化
                    SetRandomDestination(); // 新しい巡回目的地を設定
                }
                // Playerが範囲内にいる場合、UpdateMovementState()のChasePlayer()が自動的に目的地を更新する
            }
        }
    }
    
    //状態を強制的に設定し、アニメーションを即座に更新する
    public void ForceSetState(EnemyState newState)
    { 
        currentState = newState;
        UpdateAnimationState(true);
    }

    /*
    // 攻撃アニメーションを強制的に更新（AttackSequenceの冒頭などで使用）
    public void ForceUpdateAnimation()
    {
        UpdateAnimationState(true);
    }
    */
}