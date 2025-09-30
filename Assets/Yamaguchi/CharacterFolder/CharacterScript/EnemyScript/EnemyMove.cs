using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    //移動先の目標を設定する
    public Transform goal;
    private NavMeshAgent agent;
    private void Start()
    {
        
    }

    private void Update()
    {
        //NavMeshAgentコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();

        //移動先の位置を設定
        if (goal != null)
        {
            agent.destination = goal.position;
        }
    }
}
