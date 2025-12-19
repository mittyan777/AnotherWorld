using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public bool bossBattle { get; private set; }
    [SerializeField] private GameObject activeBossObj;
    [SerializeField] private GameObject activeCanvas;
    [SerializeField] private float deleteTime;

    //Boss毎の攻撃力
    //インスペクターで設定する用
    [SerializeField] private int _attackDamage01;
    [SerializeField] private int _attackDamage02;
    [SerializeField] private int _attackDamage03;

    //シーン遷移用
    [SerializeField] private string nextSceneName = "EndRoll";
    [SerializeField] private string nowSceneName = "Stage_3";
    [SerializeField] private float transitionDelay = 5.0f;


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

    private void Update()
    {
        //ボスバトル中であること
        //ボスオブジェクトが破壊(null)されたこと
        //現在のシーン名が指定のステージ名と一致すること
        if (bossBattle && activeBossObj == null)
        {
            // SceneManager.GetActiveScene().name で現在のシーン名を取得
            if (SceneManager.GetActiveScene().name == nowSceneName)
            {
                StartCoroutine(WaitAndTransition());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bossBattle)
        {
            bossBattle = true;
            BossBatrleStart(); 
        }
    }

    private void BossBatrleStart() 
    {
        activeBossObj.SetActive(true);
        activeCanvas.SetActive(true);
    }

    private IEnumerator WaitAndTransition()
    {
        // ボスが消えてから指定秒数待機
        yield return new WaitForSeconds(transitionDelay);

        // シーン移動
        SceneManager.LoadScene(nextSceneName);
    }
}
