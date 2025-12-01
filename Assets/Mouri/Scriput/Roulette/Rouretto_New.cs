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

    [SerializeField] int HP;
    [SerializeField] int MP;
    [SerializeField] private int Attack;
    [SerializeField] private int Defense;
    [SerializeField] private float  job;
    [SerializeField] GameObject GameManager;


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

    [SerializeField] private Button StartButton;
    [SerializeField] private Button StopButton;


    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;   //武器の価格
    [SerializeField] private int RouletteCost=100;  //ルーレットを回す際に必要なコスト
    [SerializeField] private bool canOnce = true;   //追加//　ルーレットを回すとき、ストップを押さないとお金を消費してルーレットを回すことを出来なくする。
    //[SerializeField] private int coin;  //所持しているお金


    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };    //文字を配列化し関数として認識させている

    [Header("状態管理")]
    private bool Spining = false;  //回転中華を判断する
    private Coroutine spinCoroutine;    //ルーレットの一時停止を行うのに適しているプログラムであり、変数として書いている

    private int[] FirstStatus = new int[4];
    private int[] UpdateStatus = new int[4];

    [Header("職種ごとの画像設定")]
    [SerializeField] private Rouletto_CG rouletto_CG;

    [Header("ルーレットごとのサウンド")]
    [SerializeField] private Roulette_Sound rouletteSound;



    void Start()
    {
        //初期UIは非表示
        RouletteUI.SetActive(false);    //ルーレットUIや背景などが「RouletteUI」に格納されておりこのルーレットUIを表示/非表示にさせるか否か

        StartButton.onClick.AddListener(StartRoulette);     
        StopButton.onClick.AddListener(StopRoulette);

        
        DontDestroyOnLoad(gameObject);

        if (rouletto_CG == null)
        {
            rouletto_CG = FindAnyObjectByType<Rouletto_CG>();//職種ごとに画像を変えるための部分
        }

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        PlayerStatus();
    }
    void Update()
    {
        PlayerStatus();

        jobtext();
    }
    void jobtext()
    {
        if(job == 0)
        {
            job_text.text = "剣士";
        }
        else if(job == 1)
        {
            job_text.text = "弓使い";
        }
        else if(job == 2)
        {
            job_text.text = "魔法使い";
        }
    }
    void UpdateCoinUI()
    {
    
        
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
        job_text.text = "職業";
    }

    void DisplayStatusOnly()//二回目以降はステータスだけの表示
    {

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();

        job_text.gameObject.SetActive(false);
        if (rouletto_CG != null)
        {   
            rouletto_CG.HideJobImage();
        }
    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }

        GameManager.GetComponent<GameManager>().Status[0] = HP;
        GameManager.GetComponent<GameManager>().Status[1] = MP;
        GameManager.GetComponent<GameManager>().Status[2] = Attack;
        GameManager.GetComponent<GameManager>().Status[3] = Defense;
        GameManager.GetComponent<GameManager>().job = job;
    }


    public void StartRoulette()
    {
        if (!canOnce) return;   //追加
        canOnce = false;    //追加

        if (FirstRoulette)
        {
            Spining = true;
            spinCoroutine = StartCoroutine(SpinRoulette());
           

            if(rouletteSound != null)
            {
                rouletteSound.rouletteSoundType = RouletteSoundType.Open;
                rouletteSound.PlaySound();
            }
       

        }
        else
        {
            if (GameManager.GetComponent<GameManager>().Coin >= RouletteCost)   //GameManegerの中にあるコインがルーレットを回すために必要なコインより多かった場合
            {
                Spining = true;
                spinCoroutine = StartCoroutine(SpinRoulette());
                GameManager.GetComponent<GameManager>().Coin -= RouletteCost;

                if (rouletteSound != null)
                {
                    rouletteSound.rouletteSoundType = RouletteSoundType.CoinUse;
                    rouletteSound.PlaySound();
                }

             
            }

            UpdateCoinUI();

            if (rouletto_CG != null)
            {
                rouletto_CG.UseCoin(RouletteCost);  //Roulett_CGの中の二回目以降にコインを消費するフェードアウト部分
            }
        }



    }
    void Status_Boost()
    {
        GameManager.GetComponent<GameManager>().HP += GameManager.GetComponent<GameManager>().Status[0];
        GameManager.GetComponent<GameManager>().MP += GameManager.GetComponent<GameManager>().Status[1];
        GameManager.GetComponent<GameManager>().AttackStatus += GameManager.GetComponent<GameManager>().Status[2];
        GameManager.GetComponent<GameManager>().DefenseStatus += GameManager.GetComponent<GameManager>().Status[3];
        GameManager.GetComponent<GameManager>().job = job;
      
    }

    public void StopRoulette()
    {
        if (!Spining) return;
        
        Spining = false;
        Status_Boost();
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);

        }
        if(rouletteSound != null)
        {
            rouletteSound.rouletteSoundType=RouletteSoundType.Stop;
            rouletteSound.PlaySound();
        }

        HP = UpdateStatus[0];
        MP = UpdateStatus[1];
        Attack = UpdateStatus[2];
        Defense = UpdateStatus[3];

        if (rouletto_CG != null)
            rouletto_CG.UpdateJobImage(jobName[(int)job], FirstRoulette);

        if(rouletteSound != null)
        {
            rouletteSound.rouletteSoundType = RouletteSoundType.Stop;
            rouletteSound.PlaySound();
        }


        ////結果を確定

        HP += Status[0];
        MP += Status[1];
        Attack += Status[2];
        Defense += Status[3];
        FirstRoulette = false;

        canOnce = true; //追加

    }

    IEnumerator SpinRoulette()
    {
        if (rouletteSound != null)
        {
            rouletteSound.rouletteSoundType = RouletteSoundType.Spin;
            rouletteSound.PlaySound();
        }
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
                job = Random.Range(0, 3);

            }

            UpdateStatusText();


            yield return new WaitForSeconds(0.05f);

            void UpdateStatusText()
            {
                HP_text.text = "HP：" + Mathf.RoundToInt(HP).ToString();
                MP_text.text = "MP：" + Mathf.RoundToInt(MP).ToString();
                Attack_text.text = "攻撃：" + Mathf.RoundToInt(Attack).ToString();
                defense_text.text = "防御：" + Mathf.RoundToInt(Defense).ToString();

            }
        }
    }
    
}
