using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class Rouretto_New : MonoBehaviour
{
    public GameObject StatusUIOnly;



    //内部ステータス（ルーレットの繁栄を行うため)

    [SerializeField] private GameObject[] Player;
    [SerializeField] private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] private float Attack;
    [SerializeField] private float Defense;
    [SerializeField] private string job;
    [SerializeField] private Image jobimage;



    [Header("ルーレット内部システム")]
    [SerializeField] public int[] Status;   //プレイヤーの配列ごとのステータスを探すための役割
    [SerializeField] private bool FirstRoulette = true;//初回かどうかの判断をするための関数
    public bool slot = true;
    private bool isActive = false;  //UIが開いているかを判断する

    private Player player; 

    [Header("UIを使用したステータス表示")]
    [SerializeField] Text Attack_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;

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


    [SerializeField] private GameObject RouletteUI;


    //[SerializeField] private Text jobText;    同じようなものが二回呼ばれている？


    //[SerializeField] private Button StartButton;
    //[SerializeField] private Button StopButton;


    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;   //武器の価格
    private int RouletteCost;
    [SerializeField] private int coin;  //所持しているお金


    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };

    [Header("状態管理")]
    private bool Spining = false;
    private Coroutine spinCoroutine;

    //[Header("所持金")]
    //public int WeponPrice;
    //private int RoulettoCost;





    // Start is called before the first frame update
    void Start()
    {
        //RouletteCost = WeponCost;     //武器屋の人と話し合い    コスト計算



        //StartButton.onClick.AddListener(StartRoulette);
        //StopButton.onClick.AddListener(StopRoulette);

        //初期UIは非表示
        RouletteUI.SetActive(false);

        DontDestroyOnLoad(gameObject);

        //RoulettoCost=GmaeM
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus();

        if (Input.GetKeyDown(KeyCode.Tab))
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
    void OpenRouletto()
    {
        //UIを開く
        isActive = true;
        RouletteUI.SetActive(true);
        //player.canControl = false;
        //Cursor.lockState= CursorLockMode.None;
        //Cursor.visible = true;

        //初回かどうかで表示が変わる
        if (FirstRoulette)
        {
            DisplayInitialRouletto();
            FirstRoulette = false;
        }
        else
        {
            DisplayStatusOnly();
        }

    }
    void CloseRouletto()
    {
        isActive = false;
        RouletteUI.SetActive(false);
        //player.canControl = false;
        //Cursor.lockState=CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    void DisplayInitialRouletto()
    {
        job_text.gameObject.SetActive(true);
        HP_text.gameObject.SetActive(true);
        MP_text.gameObject.SetActive(true);
        defense_text.gameObject.SetActive(true);
        Attack_text.gameObject.SetActive(true);

        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();
        job_text.text = Status[4].ToString();
    }


    void DisplayStatusOnly()//二回目以降はステータスだけの表示
    {
        job_text.gameObject.SetActive(false);   //SetActiveでjob部分をfalseにした。}


        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();

        FindObjectOfType<Changimage>().HideJobImage();



    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }

        Coin = Player[0].GetComponent<Player>().coin;
        Player[0].GetComponent<Player>().HP = HP;
        Player[0].GetComponent<Player>().MP = MP;
        Player[0].GetComponent<Player>().AttackStatus = Attack;
        Player[0].GetComponent<Player>().DefenseStatus = Defense;
    }
 
    public void stopslot()
    {
        slot = false;
        HP += Status[0];
        MP += Status[1];
        Attack += Status[2];
        Defense += Status[3];


        // 職種名を作る
        string jobName = "";
        if (Status[4] == 1) jobName = "剣士";
        else if (Status[4] == 2) jobName = "弓使い";
        else if (Status[4] == 3) jobName = "魔法";

        // Chang に画像切替を伝える
        FindObjectOfType<Changimage>().jobName(jobName);//職種ごとに背景を変えることが出来る

    }

    void StartRoulette()
    {
        if (Spining) return;
        if (coin < RouletteCost)
        {
            Debug.Log("お金がない");
            return;
        }

        coin -= RouletteCost;//ルーレット使用コスト
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
        if (FirstRoulette)
        {
            FirstRoulette = false;

            Debug.Log("1stルーレット");
        }

        //結果を確定
        HP = Mathf.Round(HP);
        MP = Mathf.Round(MP);
        Attack = Mathf.Round(Attack);
        Defense = Mathf.Round(Defense);

        slot = false;
        HP += Status[0];
        MP += Status[1];
        Attack += Status[2];
        Defense += Status[3];




    }

    IEnumerator SpinRoulette()
    {
        while (Spining)
        {
            // 職種は初回のみランダム決定
            if (FirstRoulette)
            {
                job = jobName[Random.Range(0, jobName.Length)];
            }
            // 各ステータスを個別にランダム化
            HP = Random.Range(HP_slot_Min, HP_slot_Max + 1);
            MP = Random.Range(MP_slot_Min, MP_slot_Max + 1);
            Attack = Random.Range(power_slot_Min, power_slot_Max + 1);
            Defense = Random.Range(Defense_slot_Min, Defense_slot_Max + 1);

            Status[4]=Random.Range(0,jobName.Length);
            job = jobName[Status[4]];

            UpdateStatusText();
            yield return new WaitForSeconds(0.05f);

        }
        void UpdateStatusText()
        {
            HP_text.text = "HP：" + Mathf.RoundToInt(HP).ToString();
            MP_text.text = "MP：" + Mathf.RoundToInt(MP).ToString();
            Attack_text.text = "攻撃：" + Mathf.RoundToInt(Attack).ToString();
            defense_text.text = "防御：" + Mathf.RoundToInt(Defense).ToString();
            job_text.text = "職業：" + job;
        }
    }
}
