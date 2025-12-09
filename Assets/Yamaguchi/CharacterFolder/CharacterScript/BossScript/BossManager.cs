using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private bool bossBattle;
    [SerializeField] private GameObject activeBossObj;
    [SerializeField] private GameObject activeCanvas;
    [SerializeField] private float deleteTime;

    //Boss毎の攻撃力
    //インスペクターで設定する用
    [SerializeField] private int _attackDamage01;
    [SerializeField] private int _attackDamage02;
    [SerializeField] private int _attackDamage03;

    //外部で渡すよう変数
    // => (ラムダ演算子) を使って、バッキングフィールドの値を直接返す
    // 外部から読み取り専用でアクセス可能。内部からも変更不可。
    public int AttackDamage01 => _attackDamage01;
    public int AttackDamage02 => _attackDamage02;
    public int AttackDamage03 => _attackDamage03;

    private void Start()
    {
        bossBattle = false;
        activeBossObj.SetActive(false);
        activeCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bossBattle)
        {
            BossBatrleStart();
            bossBattle = true;
        }
    }

    private void BossBatrleStart() 
    {
        activeBossObj.SetActive(true);
        activeCanvas.SetActive(true);
    }
}
