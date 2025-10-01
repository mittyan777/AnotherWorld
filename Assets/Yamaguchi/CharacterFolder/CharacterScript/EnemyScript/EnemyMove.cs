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
    [SerializeField]private float stayDuration;
    private float timer;
    private bool isWaiting = false;

    //================移動先変更(プレイヤー)================
    //プレイヤーを視認できる距離
    [SerializeField] private float visibilityDistance;
    [SerializeField] private Transform playerTransform;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextDestination();
    }

    private void NextDestination()
    {
        var randomPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
        agent.destination = randomPos;
    }

    private void Update()
    {
        float disutanceX = playerTransform.position.x - transform.position.x;
        float disutanceZ = playerTransform.position.z - transform.position.z;

        if (visibilityDistance < disutanceX || visibilityDistance < disutanceZ)
        {
            agent.destination = playerTransform.position;
        }
        else
        {
            if (agent.remainingDistance < 0.5f)
            {
                timer += Time.deltaTime;
                if (timer >= stayDuration)
                {
                    NextDestination();
                    timer = 0;
                }
            }
        }
    }
}
