using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMoveManager : MonoBehaviour
{
    // staticを使用し、いつでも他スクリプトから情報を見れるようにする
    public static BossMoveManager instance { get; private set; }

    [SerializeField] private GameObject playerObj;

    public int currentBossHP { get; private set; }
    private BossHP bossHP;

    // 死亡処理が一度だけ実行されたことを保証するフラグ
    private bool isDeathSequenceStarted = false;

    private GameManager gameManager;

    private TestManerger testManerger;

    //スタン設定
    [SerializeField] private string stunTag = "PlayerThunderMagic"; // スタンさせる攻撃のタグ
    [SerializeField][Range(0, 100)] private float stunProbability = 30f; // スタン確率(%)
    [SerializeField] private float stunDuration = 2.0f; // スタン時間
    private bool isStunned = false;
    //外部使用用
    public bool IsStunned => isStunned;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        bossHP = GetComponent<BossHP>();
    }

    private void Start()
    {
        GameObject test = GameObject.FindWithTag("test");
        if (test != null)
        {
            testManerger= test.GetComponent<TestManerger>();
        }

        // BossHPがAwake/StartでHPを設定しているため、ここで値を同期
        if (bossHP != null)
        {
            currentBossHP = bossHP.currentBossHP;
        }
        //重い処理だから1回だけにする
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        if (managerObject != null)
        { 
            gameManager = managerObject.GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        //テスト用のダメージ
        if (Input.GetKey(KeyCode.P))
        {
            //bossHP.TestDamage();
            StartCoroutine(bossHP.TestDamege02(100));
            // HPが変更されたらcurrentBossHPを同期
            if (bossHP != null)
            {
                currentBossHP = bossHP.currentBossHP;
            }
        }
        if (Input.GetKey(KeyCode.L))
        {
            //GameManagerから最新の攻撃力を取得する。
            int takeDamege = (int)gameManager.AttackStatus;
            //プレイヤーの攻撃力を渡す
            StartCoroutine(bossHP.TakeDamage(takeDamege));
        }
    }

    // この関数を呼び出したScriptのリストにあるfloat型のデータをリストで取得
    public int GetActionIndex(List<float> weights)
    {
        //リスト自体があるかチェック
        if (weights == null || weights.Count == 0) // nullチェックとCountチェックを修正
        {
            return -1;
        }

        //リスト内にデータがあるかチェック
        float totalWeight = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            totalWeight += weights[i];
        }

        //行動の確率が0より大きいかチェック
        if (totalWeight <= 0)
        {
            return -1;
        }

        //ランダムで行動を選抜
        float drawValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            currentWeight += weights[i];
            if (drawValue < currentWeight)
            {
                return i;
            }
        }
        // 全ての重みをチェックしても決まらない（通常は起こらない）場合は-1に戻す
        return -1;
    }

    // BossHPから呼ばれる、共通の死亡処理
    public void HandleBossDeath()
    {
        if (isDeathSequenceStarted) return;
        isDeathSequenceStarted = true;
        
        //勝手に回転しないように死亡Animationの回転を固定する。
        transform.localEulerAngles = new Vector3(0f,transform.localEulerAngles.y,0f);
        //Rigidbodyを削除 (物理挙動を止める)
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null) Destroy(body);

        BossMove01 boss01 = GetComponent<BossMove01>();
        if (boss01 != null)
        {
            //public化された ChangeState()を呼び出し、状態をDeathに変える
            boss01.ChangeState(BossMove01.Boss01ActionType.Death);
        }

        BossMove02 boss02 = GetComponent<BossMove02>();
        if (boss02 != null)
        {
            boss02.ChangeState(BossMove02.Boss02ActionType.Death);
        }

        //BossHPに遅延破壊を指示する
        if (bossHP != null)
        {
            bossHP.Die();
        }
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;
        Debug.Log("ボスがスタンしました！");

        // 各ボスのスクリプトを取得して状態をIdleにする
        BossMove01 boss01 = GetComponent<BossMove01>();
        if (boss01 != null) boss01.ChangeState(BossMove01.Boss01ActionType.Idle);

        BossMove02 boss02 = GetComponent<BossMove02>();
        if (boss02 != null) boss02.ChangeState(BossMove02.Boss02ActionType.Idle);

        // 指定時間待機
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        Debug.Log("スタンの解除");
    }

    //当たったオブジェクトにPlayerが含まれていたらダメージをくらうようにする
    // この衝突処理はプレイヤーではなくプレイヤーの攻撃タグをチェックすべきですが、元のコードを維持します。
    private void OnTriggerEnter(Collider other)
    {
        string hitTag = other.gameObject.tag;

        // --- スタン判定ロジック ---
        if (hitTag == stunTag && !isStunned && !isDeathSequenceStarted)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= stunProbability)
            {
                StartCoroutine(StunRoutine());
            }
        }


        if (other.gameObject.CompareTag("Player")) 
        { 
            return; 
        }
        if (hitTag.Contains("Player") && gameManager != null)
        {
            //GameManagerから最新の攻撃力を取得する。
            int takeDamege = (int)gameManager.AttackStatus;
            //プレイヤーの攻撃力を渡す
            StartCoroutine(bossHP.TakeDamage(takeDamege));
        }

        if (hitTag.Contains("Player"))
        {
            //GameManagerから最新の攻撃力を取得する。
            int takeDamege = testManerger.HP;
            //プレイヤーの攻撃力を渡す
            StartCoroutine(bossHP.TakeDamage(takeDamege));
        }
    }
}