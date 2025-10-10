using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    /*
    //移動先の目標を設定する
    public Transform goal;
    private NavMeshAgent agent;
    private void Start()
    {
        
    }

    private void Update()
    {
        ////NavMeshAgentコンポーネントを取得
        //agent = GetComponent<NavMeshAgent>();

        ////移動先の位置を設定
        //if (goal != null)
        //{
        //    agent.destination = goal.position;
        //}
    }*/
    private NavMeshAgent agent;
    //================待機用変数================
    [SerializeField] private float stayDuration;
    private float timer;

    //================移動先変更(プレイヤー)================
    //プレイヤーを視認できる距離
    [SerializeField] private float visibilityDistance;
    [SerializeField] private Transform playerTransform;

    //=========敵の状態を表すフラグ(アニメーション用)=========
    public enum EnemyState { Idle, Walk, Attack, None }
    public EnemyState currentState = EnemyState.Idle;

    [SerializeField] EnemyAnimationManager animationManager;

    private bool isStoppedByAttack = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animationManager = GetComponent<EnemyAnimationManager>();

        
        if (animationManager == null)
        {
            animationManager = GetComponentInChildren<EnemyAnimationManager>();
            Debug.LogError("EnemyAnimationManagerがアタッチされていません。");
        }
        if (agent == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        { 
            playerTransform = player.transform;
        }
        NextDestination();
    }

    private void NextDestination()
    {
        if (agent.enabled && !agent.isStopped)
        {
            var randomPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
            agent.destination = randomPos;
        }        
    }

    private void Update()
    {
        if (animationManager == null)
        {

        }
        else
        {
            // Managerがあれば99行目の処理を実行
            animationManager.UpdateAnimation(currentState);
        }

        if (isStoppedByAttack) 
        {
            //アニメーション遷移用のフラグ変更
            currentState = EnemyState.Attack;
            return;
        } 

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            currentState = EnemyState.Walk;
            //アニメーション遷移用のフラグ変更
            if (distanceToPlayer <= visibilityDistance) // 指定範囲内に入っているか
            {
                // プレイヤーを追跡
                agent.destination = playerTransform.position;
            }
            else
            {
                // プレイヤーが見えない、または遠すぎる場合は巡回ロジックへ
                PatrolLogic();
            }
        }
        else
        {
            // プレイヤーTransformが設定されていない場合も巡回ロジックへ
            PatrolLogic();
            currentState = EnemyState.Walk;
        }

        if (animationManager != null)
        {
            animationManager.UpdateAnimation(currentState);
        }
    }

    private void PatrolLogic()
    {
        // 目的地に到達したかチェック (NavMeshAgentがまだ計算中(pathPending)でないことを確認)
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentState = EnemyState.Idle;
            timer += Time.deltaTime;
            //アニメーション遷移用のフラグ変更
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
    }

    public void SetIsAttacking(bool isAttacking)
    {
        isStoppedByAttack = isAttacking;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = isAttacking;

            // 再開時 (isAttackingがfalseになったとき) のみ、すぐに目的地を更新
            if (!isAttacking)
            {
                // プレイヤーが見えていれば追跡、見えていなければ巡回を再開
                if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= visibilityDistance)
                {
                    agent.SetDestination(playerTransform.position);
                }
                else
                {
                    NextDestination();
                }
            }
        }
    }
}
