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
        // BossHPがAwake/StartでHPを設定しているため、ここで値を同期
        if (bossHP != null)
        {
            currentBossHP = bossHP.currentBossHP;
        }
    }

    private void Update()
    {
        //テスト用のダメージ
        if (Input.GetKey(KeyCode.P))
        {
            bossHP.TestDamage();
            // HPが変更されたらcurrentBossHPを同期
            if (bossHP != null)
            {
                currentBossHP = bossHP.currentBossHP;
            }
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

    //当たったオブジェクトにPlayerが含まれていたらダメージをくらうようにする
    // この衝突処理はプレイヤーではなくプレイヤーの攻撃タグをチェックすべきですが、元のコードを維持します。
    private void OnTriggerEnter(Collider other)
    {
        string hitTag = other.gameObject.tag;
        if (other.gameObject.CompareTag("Player")) 
        { 
            return; 
        } 
        if (hitTag.Contains("Player"))
        {
            bossHP.TestDamage();
        }
    }
}