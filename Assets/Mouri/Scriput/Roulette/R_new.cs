using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class R_new : MonoBehaviour
{
    public GameObject StatusUIOnly;
    Changimage changimage;

    [SerializeField] private GameObject[] Player;
    [SerializeField] private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] private float Attack;
    [SerializeField] private float Defense;
    [SerializeField] private string job;
    [SerializeField] private Image jobimage;

    [Header("ルーレット内部システム")]
    [SerializeField] public int[] Status;
    [SerializeField] private bool FirstRoulette = true;
    public bool slot = true;
    private bool isActive = false;

    private Player player;

    [Header("UIを使用したステータス表示")]
    [SerializeField] Text Attack_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;

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
    [SerializeField] private Button StartButton;

    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;
    private int RouletteCost;
    [SerializeField] private int coin;

    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };

    [Header("背景設定")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite swordsmanBG;
    [SerializeField] private Sprite mageBG;
    [SerializeField] private Sprite archerBG;

    [Header("状態管理")]
    private bool Spining = false;
    private Coroutine spinCoroutine;

    // 初回ステータスと累積ステータス
    private int[] initialStatus = new int[4];
    private int[] accumulatedStatus = new int[4];

    void Start()
    {
        StartButton.onClick.AddListener(StartRoulette);
        changimage = FindObjectOfType<Changimage>();

        RouletteUI.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        PlayerStatus();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isActive)
                CloseRouletto();
            else
            {
                OpenRouletto();
                PlayerStatus();
            }
        }
    }

    void OpenRouletto()
    {
        isActive = true;
        RouletteUI.SetActive(true);

        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            Spining = false;
        }

        if (FirstRoulette)
        {
            DisplayInitialRouletto();
            FirstRoulette = false;
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

    void DisplayStatusOnly()
    {
        job_text.gameObject.SetActive(false);

        HP_text.text = accumulatedStatus[0].ToString();
        MP_text.text = accumulatedStatus[1].ToString();
        Attack_text.text = accumulatedStatus[2].ToString();
        defense_text.text = accumulatedStatus[3].ToString();

        if (changimage != null)
            changimage.HideJobImage();
    }

    private void PlayerStatus()
    {
        if (Player[0] == null)
            Player = GameObject.FindGameObjectsWithTag("Player");

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

        coin -= RouletteCost;
        Spining = true;
        spinCoroutine = StartCoroutine(SpinRoulette());
    }

    void StopRoulette()
    {
        if (!Spining) return;

        Spining = false;

        if (spinCoroutine != null)
            StopCoroutine(spinCoroutine);

        if (!FirstRoulette)
        {
            accumulatedStatus[0] += Status[0];
            accumulatedStatus[1] += Status[1];
            accumulatedStatus[2] += Status[2];
            accumulatedStatus[3] += Status[3];
        }

        HP = accumulatedStatus[0];
        MP = accumulatedStatus[1];
        Attack = accumulatedStatus[2];
        Defense = accumulatedStatus[3];

        slot = false;
        FirstRoulette = false;
    }

    IEnumerator SpinRoulette()
    {
        while (Spining)
        {
            int hpRand = Random.Range(HP_slot_Min, HP_slot_Max + 1);
            int mpRand = Random.Range(MP_slot_Min, MP_slot_Max + 1);
            int attackRand = Random.Range(power_slot_Min, power_slot_Max + 1);
            int defenseRand = Random.Range(Defense_slot_Min, Defense_slot_Max + 1);

            if (FirstRoulette)
            {
                // 初回ステータス決定
                initialStatus[0] = hpRand;
                initialStatus[1] = mpRand;
                initialStatus[2] = attackRand;
                initialStatus[3] = defenseRand;

                accumulatedStatus[0] = initialStatus[0];
                accumulatedStatus[1] = initialStatus[1];
                accumulatedStatus[2] = initialStatus[2];
                accumulatedStatus[3] = initialStatus[3];

                Status[4] = Random.Range(0, jobName.Length);
                job = jobName[Status[4]];

                // 背景画像切替
                if (job == "剣士") backgroundImage.sprite = swordsmanBG;
                if (job == "魔法使い") backgroundImage.sprite = mageBG;
                if (job == "弓使い") backgroundImage.sprite = archerBG;
            }
            else
            {
                // 2回目以降は累積用にStatus更新
                Status[0] = hpRand;
                Status[1] = mpRand;
                Status[2] = attackRand;
                Status[3] = defenseRand;
            }

            UpdateStatusText(hpRand, mpRand, attackRand, defenseRand);

            yield return new WaitForSeconds(0.05f);
        }
    }

    void UpdateStatusText(int hp, int mp, int attack, int defense)
    {
        HP_text.text = "HP：" + (accumulatedStatus[0] + hp).ToString();
        MP_text.text = "MP：" + (accumulatedStatus[1] + mp).ToString();
        Attack_text.text = "攻撃：" + (accumulatedStatus[2] + attack).ToString();
        defense_text.text = "防御：" + (accumulatedStatus[3] + defense).ToString();

        if (FirstRoulette)
            job_text.text = "職業：" + job;
    }

}

