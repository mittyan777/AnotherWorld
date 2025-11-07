using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class Rouretto_New : MonoBehaviour
{
    //public GameObject StatusUIOnly;

    //Changimage changimage;

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
    public bool slot = true;
    private bool isActive = false;  //UIが開いているかを判断する

    

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
    [SerializeField] private Image Rouret_back; //半透明


    //[SerializeField] private Text jobText;    同じようなものが二回呼ばれている？


    [SerializeField] private Button StartButton;
    [SerializeField] private Button StopButton;


    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;   //武器の価格
    private int RouletteCost;
    [SerializeField] private int coin;  //所持しているお金


    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };

    [Header("状態管理")]
    private bool Spining = false;
    private Coroutine spinCoroutine;

    private int[] FirstStatus = new int[4];
    private int[] UpdateStatus = new int[4];

    [Header("職種ごとの画像設定")]
    [SerializeField] private Changimage changimage;



    //[Header("所持金")]
    //public int WeponPrice;
    //private int RoulettoCost;

    // Start is called before the first frame update
    void Start()
    {
        //RouletteCost = WeponCost;     //武器屋の人と話し合い    コスト計算

        RouletteUI.SetActive(false);
        if (Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(false);
        }

        StartButton.onClick.AddListener(StartRoulette);
        StopButton.onClick.AddListener(StopRoulette);

        //changimage = FindObjectOfType<Changimage>();

        //初期UIは非表示
        RouletteUI.SetActive(false);

        DontDestroyOnLoad(gameObject);


        //RoulettoCost=GmaeM
        if (changimage == null)
        {
            changimage = FindAnyObjectByType<Changimage>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (slot == true)
            {
                Status[0] = Random.Range(HP_slot_Min, HP_slot_Max);
                Status[1] = Random.Range(MP_slot_Min, MP_slot_Max);
                Status[2] = Random.Range(power_slot_Min, power_slot_Max);
                Status[3] = Random.Range(Defense_slot_Min, Defense_slot_Max);
                Status[4] = Random.Range(0,jobName.Length);


            }

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
        if (Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(true);
        }
        //player.canControl = false;
        //Cursor.lockState= CursorLockMode.None;
        //Cursor.visible = true;

        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            Spining = false;
        }

        //初回かどうかで表示が変わる
        if (FirstRoulette)
        {
            DisplayInitialRouletto();
            //FirstRoulette = false;
        }
        else
        {
            DisplayStatusOnly();
        }

        StartRoulette();

    }

    void CloseRouletto()
    {
        isActive = false;
        RouletteUI.SetActive(false);

        if(Rouret_back != null)
        {
            Rouret_back.gameObject.SetActive(false);
        }
        //player.canControl = false;
        //Cursor.lockState=CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    void DisplayInitialRouletto()   //一番最初のステータスUIの表示
    {
        //job_text.gameObject.SetActive(true);
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
        //job_text.text = Status[4].ToString();
    }

    void DisplayStatusOnly()//二回目以降はステータスだけの表示
    {
        //job_text.gameObject.SetActive(false);   //SetActiveでjob部分をfalseにした。}


        HP_text.text = Status[0].ToString();
        MP_text.text = Status[1].ToString();
        Attack_text.text = Status[2].ToString();
        defense_text.text = Status[3].ToString();

        //if (changimage != null)
        //{
        //    changimage.HideJobImage();

        //}
        job_text.gameObject.SetActive(false);
        if (changimage != null)
        {
            changimage.HideJobImage();
        }
        //FindObjectOfType<Changimage>().HideJobImage();


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
        //if (!FirstRoulette)
        //{

        //    UpdateStatus[0] += Status[0];
        //    UpdateStatus[1] += Status[1];
        //    UpdateStatus[2] += Status[2];
        //    UpdateStatus[3] += Status[3];

        //    FirstRoulette = false;

        //    Debug.Log("1stルーレット");
        //}

        HP = UpdateStatus[0];
        MP = UpdateStatus[1];
        Attack = UpdateStatus[2];
        Defense = UpdateStatus[3];


        if (changimage != null && FirstRoulette) 
            changimage.UpdateJobImage(job, FirstRoulette);

        slot = false;




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

                ////////職種ごとの画像変更
                //////if (job == "剣士")
                //////{
                //////    jobimage.sprite = swordsman;
                //////}
                //////else if(job == "魔法使い")
                //////{
                //////    jobimage.sprite = Magishan;
                //////}

                //////else if( job == "弓使い")
                //////{
                //////    jobimage.sprite = Aceher;
                //////}

                //////jobimage.gameObject.SetActive(true);

                //////FirstRoulette = false;

                //    if(job=="剣士")Image.sprite=
                //};

                //if(jobName==剣士")Image.sprit

                
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
            }
        }
    }
}

