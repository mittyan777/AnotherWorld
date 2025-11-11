using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class Rouretto_New : MonoBehaviour
{


    //内部ステータス（ルーレットの繁栄を行うため)

    [SerializeField] private GameObject[] Player;
    private Player player;

    [SerializeField] private float Coin;
    [SerializeField] int HP;
    [SerializeField] int MP;
    [SerializeField] private int Attack;
    [SerializeField] private int Defense;
    [SerializeField] private string job;
    


    [Header("ルーレット内部システム")]
    [SerializeField] public int[] Status;   //プレイヤーの配列ごとのステータスを探すための役割
    [SerializeField] private bool FirstRoulette = true;//初回かどうかの判断をするための関数
    
    private bool isActive = false;  //UIが開いているかを判断する

    

    [Header("UIを使用したステータス表示")]
    [SerializeField] Text Attack_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;
    [SerializeField] Text Coin_Text;

    //[SerializeField] Text Player_Coin;

    [Header("ステータスそれぞれの最小・最大(ルーレット時）")]

    [SerializeField] int HP_slot_Max;
    [SerializeField] int HP_slot_Min;

    [SerializeField] int MP_slot_Max;
    [SerializeField] int MP_slot_Min;

    [SerializeField] int power_slot_Max;
    [SerializeField] int power_slot_Min;

    [SerializeField] int Defense_slot_Max;
    [SerializeField] int Defense_slot_Min;


    //ルーレットUI要素
    [SerializeField] private GameObject RouletteUI;
    [SerializeField] private Image Rouret_back; //半透明


    //[SerializeField] private Text jobText;    同じようなものが二回呼ばれている？


    [SerializeField] private Button StartButton;
    [SerializeField] private Button StopButton;


    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;   //武器の価格
    [SerializeField] private int RouletteCost=100;  //ルーレットを回す際に必要なコスト
    //[SerializeField] private int coin;  //所持しているお金


    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };    //文字を配列化し関数として認識させている

    [Header("状態管理")]
    private bool Spining = false;  //回転中華を判断する
    private Coroutine spinCoroutine;    //ルーレットの一時停止を行うのに適しているプログラムであり、変数として書いている

    private int[] FirstStatus = new int[4];
    private int[] UpdateStatus = new int[4];

    [Header("職種ごとの画像設定")]
    [SerializeField] private Changimage changimage;

    void Start()
    {
        //初期UIは非表示
        RouletteUI.SetActive(false);    //ルーレットUIや背景などが「RouletteUI」に格納されておりこのルーレットUIを表示/非表示にさせるか否か

        if (Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(false);
        }

        StartButton.onClick.AddListener(StartRoulette);     
        StopButton.onClick.AddListener(StopRoulette);

        
        DontDestroyOnLoad(gameObject);

        if (changimage == null)
        {
            changimage = FindAnyObjectByType<Changimage>();//職種ごとに画像を変えるための部分
        }
    }
    void Update()
    {
        PlayerStatus();

        if (Input.GetKeyDown(KeyCode.Tab))  //もしもTabキーを押すと下のプログラムを呼び出す
        {

            if (isActive)
            {
                CloseRouletto();    //UIが開いていればTabキーを押して閉じる
            }
            else
            {
                OpenRouletto();
                PlayerStatus();
            }

        }

    }
    void UpdateCoinUI()
    {
        if (Coin < 0)
        {
            Coin = 0;
        }
        Coin_Text.text = "所持金:" + Coin.ToString();

    }
    void OpenRouletto()
    {
        //UIを開く
        isActive = true;
        RouletteUI.SetActive(true);
        if (Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(true);
        }

        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            Spining = false;
        }

        //初回かどうかで表示が変わる
        if (FirstRoulette)
        {

            DisplayInitialRouletto();

        }
        else
        {
            DisplayStatusOnly();

        }

        UpdateCoinUI();
    }

    void CloseRouletto()
    {
        isActive = false;
        RouletteUI.SetActive(false);

        if(Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(false);
        }
    }
    void DisplayInitialRouletto()   //一番最初のステータスUIの表示
    {
        HP_text.gameObject.SetActive(true);
        MP_text.gameObject.SetActive(true);
        defense_text.gameObject.SetActive(true);
        Attack_text.gameObject.SetActive(true);

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();

        job_text.gameObject.SetActive(true) ;
        job_text.text = "職業"+job;
    }

    void DisplayStatusOnly()//二回目以降はステータスだけの表示
    {

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();

        job_text.gameObject.SetActive(false);
        if (changimage != null)
        {
            changimage.HideJobImage();
        }
    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }

        Player[0].GetComponent<Player>().HP = HP;
        Player[0].GetComponent<Player>().MP = MP;
        Player[0].GetComponent<Player>().AttackStatus = Attack;
        Player[0].GetComponent<Player>().DefenseStatus = Defense;
    }


    void StartRoulette()
    {
        if (Spining) return;

        if(!FirstRoulette)
        {

            if (Coin <RouletteCost)
            {
                
                UpdateCoinUI();
                Debug.Log("お金がない");
                return;
            }

            Debug.Log("coinBe" + Coin);

            Coin -= RouletteCost;//ルーレット使用コスト

            Debug.Log("coinAf" + Coin);


            Player[0].GetComponent<Player>().coin = Coin;

            UpdateCoinUI();

        }

        Spining = true;

        spinCoroutine = StartCoroutine(SpinRoulette());

    }

    void StopRoulette()
    {
        if (!Spining) return;

        Spining = false;

        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);

        }

        HP = UpdateStatus[0];
        MP = UpdateStatus[1];
        Attack = UpdateStatus[2];
        Defense = UpdateStatus[3];


        if (changimage != null && FirstRoulette) 
            changimage.UpdateJobImage(job, FirstRoulette);

        ////結果を確定

        HP += Status[0];
        MP += Status[1];
        Attack += Status[2];
        Defense += Status[3];
        FirstRoulette = false;

    }

    IEnumerator SpinRoulette()
    {
        while (Spining)
        {
            // 各ステータスを個別にランダム化
             HP = Random.Range(HP_slot_Min, HP_slot_Max + 1);
             MP = Random.Range(MP_slot_Min, MP_slot_Max + 1);
             Attack = Random.Range(power_slot_Min, power_slot_Max + 1);
             Defense = Random.Range(Defense_slot_Min, Defense_slot_Max + 1);

            // 職種は初回のみランダム決定
            if (FirstRoulette)
            {
                Status[4]=Random.Range(0,jobName.Length);
                job=jobName[Status[4]];

                FirstStatus[0] = (int)HP;
                FirstStatus[1] = (int)MP;
                FirstStatus[2] = (int)Attack;
                FirstStatus[3] = (int)Defense;

                UpdateStatus[0] = FirstStatus[0];
                UpdateStatus[1] = FirstStatus[1];
                UpdateStatus[2] = FirstStatus[2];
                UpdateStatus[3] = FirstStatus[3];

                Status[4] = Random.Range(0, jobName.Length);

                job = jobName[Status[4]];

            }

            UpdateStatusText();

            yield return new WaitForSeconds(0.05f);

            void UpdateStatusText()
            {
                HP_text.text = "HP：" + Mathf.RoundToInt(HP).ToString();
                MP_text.text = "MP：" + Mathf.RoundToInt(MP).ToString();
                Attack_text.text = "攻撃：" + Mathf.RoundToInt(Attack).ToString();
                defense_text.text = "防御：" + Mathf.RoundToInt(Defense).ToString();
                job_text.text = "職業：" + job;
                //Coin_Text.text = "所持金";
            }
        }
    }
}
