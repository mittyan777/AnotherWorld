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

    private GameManager gameManager;
    private TestManerger testManerger;

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

        GameObject managerObject = GameObject.FindWithTag("GameManager");
        if (managerObject != null)
        {
            gameManager = managerObject.GetComponent<GameManager>();
        }

        //テストコード
        GameObject test = GameObject.FindWithTag("test");
        if (test != null)
        {
            testManerger = test.GetComponent<TestManerger>();
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
        //if (enemyCurrentHP <= 0) return;
        //TODO:ここではPlayerから受け取ったダメージを入れる。

        //本来のコード
        enemyCurrentHP -= (int)gameManager.AttackStatus;

        //テストコード
        //enemyCurrentHP -= testManerger.AP;

        //enemyCurrentHP -= 5;
        //Debug.Log(testManerger.AP);
        Debug.Log(enemyCurrentHP);

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
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (other.gameObject.tag.Contains("Player"))
        {
            GetDamege();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerFireMagic")
        {
            GetDamege();
        }
    }
}
