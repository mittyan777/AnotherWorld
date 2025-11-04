using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private int enemyMaxHP;
    private int enemyCurrentHP;
    EnemyDropManager dropManager;

    protected EnemyMove enemyMove;
    [SerializeField] private float damageTime;
    [SerializeField] private float breakTime;

    private void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        dropManager = GetComponent<EnemyDropManager>();

        enemyMove = GetComponentInParent<EnemyMove>();
        if (enemyMove == null)
        {
            Debug.LogError(gameObject.name + ": EnemyMoveが見つかりません。", this);
            this.enabled = false;
            return;
        }
    }

    private void Update()
    {

        //テスト用ダメージ
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetDamege();
            Debug.Log("Enemyに5のダメージ");
        }
    }

    private void EnemyDied()
    {
        enemyMove.ForceSetState(EnemyMove.EnemyState.death);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;  // 即座に移動を停止
        }

        enemyMove.enabled = false;
        // 死亡処理をコルーチンで実行
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        if (dropManager != null)
        {
            dropManager.DropItem();
        }
        yield return new WaitForSeconds(damageTime);
        Destroy(gameObject, breakTime);
    }

    private IEnumerator DamageSequence() 
    {
        enemyMove.SetIsStoppedByAttack(true);

        yield return new WaitForSeconds(damageTime);

        if (enemyCurrentHP > 0)
        {
            enemyMove.ForceSetState(EnemyMove.EnemyState.Idle);

            if (enemyMove.GetComponent<NavMeshAgent>() != null)
            {
                enemyMove.GetComponent<NavMeshAgent>().ResetPath();
            }

            enemyMove.SetIsStoppedByAttack(false);
        }
    }

    private void GetDamege()
    {
        if (enemyMove.currentState == EnemyMove.EnemyState.death) return;
        if (enemyCurrentHP <= 0) return;
        //TODO:ここではPlayerから受け取ったダメージを入れる。
        enemyCurrentHP -= 5;

        //死亡しているのでダメージのアニメーションは不要
        if (enemyCurrentHP <= 0)
        {
            //enemyMove.currentState = EnemyMove.EnemyState.death;
            //死亡状態になったら、他のコルーチンを全て停止
            StopAllCoroutines();
            EnemyDied();
            return;
        }

        //HPが0じゃ無ければ生きているという事なのでダメージの処理をする
        if (enemyMove.currentState != EnemyMove.EnemyState.damage)
        { 
            enemyMove.currentState = EnemyMove.EnemyState.damage;
            StopAllCoroutines();

            enemyMove.ForceSetState(EnemyMove.EnemyState.damage);

            StartCoroutine(DamageSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            GetDamege();
        }
        else 
        {
            return;
        }
    }
}
