using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RourettoManejer : MonoBehaviour
{

    [Header("UI関連")]

    [SerializeField] private GameObject[] Player;
    [SerializeField] private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] float AttackStatus;
    [SerializeField] float DefenseStatus;
    [SerializeField] public float job;

    [SerializeField] public int[] Status;
    public bool slot = true;    //スロットを回すのに仏用
    [SerializeField] Text powerslot_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;

    [SerializeField] int HP_slot_Max;
    [SerializeField] int HP_slot_Min;

    [SerializeField] int MP_slot_Max;
    [SerializeField] int MP_slot_Min;

    [SerializeField] int power_slot_Max;
    [SerializeField] int power_slot_Min;

    [SerializeField] int Defense_slot_Max;
    [SerializeField] int Defense_slot_Min;

    [SerializeField] private GameObject RourettoUI;
    [SerializeField] private Text jobText;

    [Header("お金")]
    public int weponPrice = 1000;
    private int RoulettoCost;

    [Header("ルーレット制御")]
    private bool FirstRoulette = true;    //ルーレットが初回かどうかを判断する。
    private bool isAsctive = false;       //UIが開いているかどうか。

    public GameObject roulettoUI;   //ルーレットのUI全体

    private Player player; //プレイヤースクリプトを参照するための部分


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //RoulettoCost = GameManger_Mouri.weaponCost / 50f;

        //UpdateCoinDisplayer();

        
    }
    private void Update()
    {
        // ------------------------------
        // ① Tabキーでルーレット開閉処理
        //    ※開閉条件を修正しました（前は逆になっていた）
        // ------------------------------
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isAsctive)
            {
                CloseRoulette();  // UIが開いていれば閉じる
            }
            else
            {
                OpenRoulette();   // 閉じていれば開く
                PlayerStatus();   // プレイヤーステータス更新（開いたときに一度だけ）
            }
        }

        // ------------------------------
        // ② ステータススロット処理
        // ------------------------------
        if (slot == true)
        {
            Status[0] = Random.Range(HP_slot_Min, HP_slot_Max);
            Status[1] = Random.Range(MP_slot_Min, MP_slot_Max);
            Status[2] = Random.Range(power_slot_Min, power_slot_Max);
            Status[3] = Random.Range(Defense_slot_Min, Defense_slot_Max);
            Status[4] = Random.Range(1, 5);
        }

        // ------------------------------
        // ③ UI更新
        // ------------------------------
        HP_text.text = $"{Status[0]}";
        MP_text.text = $"{Status[1]}";
        powerslot_text.text = $"{Status[2]}";
        defense_text.text = $"{Status[3]}";

        // 職業テキスト
        if (Status[4] == 1) { job_text.text = "剣士"; }
        if (Status[4] == 2) { job_text.text = "アーチャー"; }
        if (Status[4] == 3) { job_text.text = "マジシャン"; }
    }


    // ------------------------------
    // プレイヤーステータスを更新
    // ------------------------------
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }

        Coin = Player[0].GetComponent<Player>().coin;
        Player[0].GetComponent<Player>().HP = HP;
        Player[0].GetComponent<Player>().MP = MP;
        Player[0].GetComponent<Player>().AttackStatus = AttackStatus;
        Player[0].GetComponent<Player>().DefenseStatus = DefenseStatus;
    }


    // ------------------------------
    // スロット停止
    // ------------------------------
    public void stopslot()
    {
        slot = false;
        HP += Status[0];
        MP += Status[1];
        AttackStatus += Status[2];
        DefenseStatus += Status[3];

        // 職業名を生成
        string jobName = GetJobName(Status[4]);

        // 背景画像変更（Changimageへ伝える）
        FindObjectOfType<Changimage>().jobName(jobName);
    }


    // ------------------------------
    // ルーレットを開く処理
    // ------------------------------
    void OpenRoulette()
    {
        // UIを開く
        isAsctive = true;
        roulettoUI.SetActive(true);
        player.canControl = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 初回かどうかで表示切り替え
        if (FirstRoulette)
        {
            DisplayInitialRoulette();
            FirstRoulette = false;
        }
        else
        {
            DisplayStatusOnly();
        }
    }


    // ------------------------------
    // ルーレットを閉じる処理
    // ------------------------------
    void CloseRoulette()
    {
        isAsctive = false;
        roulettoUI.SetActive(false);
        player.canControl = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // ------------------------------
    // 初回ルーレット表示（ジョブ＋ステータス）
    // ------------------------------
    void DisplayInitialRoulette()
    {
        job_text.gameObject.SetActive(true);
        HP_text.gameObject.SetActive(true);
        MP_text.gameObject.SetActive(true);
        powerslot_text.gameObject.SetActive(true);
        defense_text.gameObject.SetActive(true);

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        powerslot_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();
        job_text.text = GetJobName(Status[4]);

    }


    // ------------------------------
    // 2回目以降ルーレット表示（ステータスのみ）
    // ------------------------------
    void DisplayStatusOnly()
    {
        job_text.gameObject.SetActive(false);

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        powerslot_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();
    }


    // ------------------------------
    // 職業名を返す関数（修正版）
    // ------------------------------
    string GetJobName(int obj)
    {
        // 🟢 修正版: 以前は job_text.text を返すだけで無意味だった
        if (obj == 1) return "剣士";
        else if (obj == 2) return "アーチャー";
        else if (obj == 3) return "マジシャン";
        else return "不明";
    }
    
 
}

// Update is called once per frame
