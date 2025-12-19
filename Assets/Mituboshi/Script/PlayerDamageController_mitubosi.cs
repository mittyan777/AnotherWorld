using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController_mitubosi : MonoBehaviour
{
    private GameManager gameManager;
    private EnemyDamageManager enemyDamageManager;
    private BossManager bossManager;
    private TestManerger testManerger;

    //Playerが受ける無敵時間
    [SerializeField] private float invincibilityDuration = 0.5f;
    private float invincibilityTimer = 0f;

    //ダメージを与える敵の攻撃タグ一覧
    [Tooltip("雑魚敵の攻撃タグ")]
    [SerializeField] private string goblinAttack01 = "GoblinAttack01";
    [SerializeField] private string goblinAttack03 = "GoblinAttack03";
    [SerializeField] private string skeletonAttack01 = "SkeletonAttack01";
    [SerializeField] private string skeletonAttack03 = "SkeletonAttack03";
    [SerializeField] private string ThrowadleAttack = "ThrowadleAttack";
    [SerializeField] private string spiderAttack01 = "SpiderAttack01";
    [SerializeField] private string spiderAttack02 = "SpiderAttack02";
    [SerializeField] private string wolfAttack01 = "WolfAttack";
    [Tooltip("ボスの攻撃タグ")]
    [SerializeField] private string bossAttackTag01 = "AttackArea01";
    [SerializeField] private string bossAttackTag02 = "AttackArea02";
    [SerializeField] private string bossAttackTag03 = "AttackArea03";


    private void Start()
    {
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        if (managerObject != null)
        {
            gameManager = managerObject.GetComponent<GameManager>();
        }

        //ダメージマネージャーを取得
        enemyDamageManager = FindObjectOfType<EnemyDamageManager>();
        bossManager = FindObjectOfType<BossManager>();

        //テストコード
        GameObject test = GameObject.FindWithTag("test");
        if (test != null)
        {
            testManerger = test.GetComponent<TestManerger>();
        }
        if (testManerger == null)
        {
            Debug.Log("ない");
        }

        if (gameManager == null || enemyDamageManager == null || bossManager == null)
        {
            Debug.LogError("必要なマネージャーが見つかりません。PlayerHealthControllerを無効化します。", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (invincibilityTimer > 0) return;

        int damage = 0;
        string tag = other.tag;

        // --- 雑魚敵の攻撃判定 ---
        if (tag == goblinAttack01)
        {
            damage = enemyDamageManager.GoblinAttackDamage01;
        }
        else if (tag == goblinAttack03)
        {
            damage = enemyDamageManager.GoblinAttackDamage03;
        }
        else if (tag == skeletonAttack01)
        {
            damage = enemyDamageManager.SkeletonAttackDamage01;
        }
        else if (tag == skeletonAttack03)
        {
            damage = enemyDamageManager.SkeletonAttackDamage03;
        }
        else if (tag == ThrowadleAttack)
        {
            damage = enemyDamageManager.ThrowanleAttackDamage;
        }
        else if (tag == spiderAttack01)
        {
            damage = enemyDamageManager.SpiderAttackDamage01;
        }
        else if (tag == spiderAttack02)
        {
            damage = enemyDamageManager.SpiderAttackDamage02;
        }
        else if (tag == wolfAttack01)
        {
            damage = enemyDamageManager.WolfAttackDamage;
        }

        // --- ボスの攻撃判定 ---
        else if (tag == bossAttackTag01)
        {
            damage = bossManager.AttackDamage01;
        }
        else if (tag == bossAttackTag02)
        {
            damage = bossManager.AttackDamage02;
        }
        else if (tag == bossAttackTag03)
        {
            damage = bossManager.AttackDamage03;
        }

        // --- ダメージ適用処理 ---
        if (damage > 0)
        {
            //GameManagerの公開メソッドを呼び出し、HP減少を依頼 ★
            if (gameManager != null)
            {
                //testManerger.testDamage(damage);
                gameManager.TakeDamage(damage);
            }
            // 無敵時間をリセット
            invincibilityTimer = invincibilityDuration;
        }
    }
}