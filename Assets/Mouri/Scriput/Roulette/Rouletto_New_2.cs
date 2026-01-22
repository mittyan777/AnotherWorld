using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class Rouletto_New_2 : MonoBehaviour
{

    [SerializeField] private GameObject[] Player;
    private Player player;

    [SerializeField] int HP;
    [SerializeField] int MP;
    [SerializeField] private int Attack;
    [SerializeField] private int Defense;
    [SerializeField] private float job;
    [SerializeField] GameObject GameManager;

    [Header("ルーレット内部システム")]
    [SerializeField] public int[] Status;
    [SerializeField] public bool FirstRoulette = true;

    private bool isActive = false;

    [Header("UI表示")]
    [SerializeField] Text Attack_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;
    [SerializeField] Text Coin_Text;

    [Header("ステータス範囲")]
    [SerializeField] int HP_slot_Max;
    [SerializeField] int HP_slot_Min;
    [SerializeField] int MP_slot_Max;
    [SerializeField] int MP_slot_Min;
    [SerializeField] int power_slot_Max;
    [SerializeField] int power_slot_Min;
    [SerializeField] int Defense_slot_Max;
    [SerializeField] int Defense_slot_Min;

    [Header("UI要素")]
    [SerializeField] private GameObject RouletteUI;
    [SerializeField] private Image Rouret_back;

    [SerializeField] private Button StartButton;
    [SerializeField] private Button StopButton;

    [Header("ルーレット設定")]
    [SerializeField] private float WeponCost;
    [SerializeField] private int RouletteCost = 100;
    private bool canOnce = true; // ルーレットボタン押せるか
    private bool Spining = false; // 回転中判定
    private Coroutine spinCoroutine;

    private int[] UpdateStatus = new int[4];

    [Header("職種設定")]
    [SerializeField] private string[] jobName = { "剣士", "魔法使い", "弓使い" };

    [Header("職種ごとの画像設定")]
    [SerializeField] private Rouletto_CG rouletto_CG;

    [Header("ルーレットサウンド")]
    [SerializeField] private Roulette_Sound rouletteSound;

    void Start()
    {
        RouletteUI.SetActive(false);

        StartButton.onClick.AddListener(StartRoulette);
        StopButton.onClick.AddListener(StopRoulette);

        DontDestroyOnLoad(gameObject);

        if (rouletto_CG == null)
            rouletto_CG = FindAnyObjectByType<Rouletto_CG>();

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        PlayerStatus();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "load_screen")
            PlayerStatus();

        UpdateJobText();
    }

    void UpdateJobText()
    {
        if (job == 0) job_text.text = "剣士";
        else if (job == 1) job_text.text = "弓使い";
        else if (job == 2) job_text.text = "魔法使い";
    }

    private void PlayerStatus()
    {
        if (Player.Length == 0 || Player[0] == null)
            Player = GameObject.FindGameObjectsWithTag("Player");

        GameManager.GetComponent<GameManager>().Status[0] = HP;
        GameManager.GetComponent<GameManager>().Status[1] = MP;
        GameManager.GetComponent<GameManager>().Status[2] = Attack;
        GameManager.GetComponent<GameManager>().Status[3] = Defense;
        GameManager.GetComponent<GameManager>().job = job;
    }

    void UpdateCoinUI()
    {
        if (Coin_Text != null)
            Coin_Text.text = GameManager.GetComponent<GameManager>().Coin.ToString();
    }

    public void StartRoulette()
    {
        // 回ってる最中は連打不可
        if (!canOnce || Spining)
            return;

        // コイン不足の場合
        if (!FirstRoulette && GameManager.GetComponent<GameManager>().Coin < RouletteCost)
        {
            canOnce = true; // ボタン押せる状態に戻す
            return;
        }

        canOnce = false; // ここから連打禁止
        Spining = true;

        // 初回ルーレットは職種も決定
        if (FirstRoulette)
        {
            spinCoroutine = StartCoroutine(SpinRoulette());
            if (rouletteSound != null)
            {
                rouletteSound.rouletteSoundType = RouletteSoundType.Open;
                rouletteSound.PlaySound();
            }
        }
        else
        {
            // コイン消費して回す
            GameManager.GetComponent<GameManager>().Coin -= RouletteCost;
            UpdateCoinUI();

            if (rouletto_CG != null)
                rouletto_CG.UseCoin(RouletteCost);

            spinCoroutine = StartCoroutine(SpinRoulette());

            if (rouletteSound != null)
            {
                rouletteSound.rouletteSoundType = RouletteSoundType.CoinUse;
                rouletteSound.PlaySound();
            }
        }
    }

    public void StopRoulette()
    {
        if (!Spining) return;

        Spining = false;
        canOnce = true; // 停止したら再度ボタン押せる

        if (spinCoroutine != null)
            StopCoroutine(spinCoroutine);

        // ステータス確定
        Status_Boost();

        HP = UpdateStatus[0];
        MP = UpdateStatus[1];
        Attack = UpdateStatus[2];
        Defense = UpdateStatus[3];

        if (rouletto_CG != null)
            rouletto_CG.UpdateJobImage(jobName[(int)job], FirstRoulette);

        if (rouletteSound != null)
        {
            rouletteSound.rouletteSoundType = RouletteSoundType.Stop;
            rouletteSound.PlaySound();
        }

        // 初回ルーレット確定
        FirstRoulette = false;
    }

    void Status_Boost()
    {
        GameManager.GetComponent<GameManager>().Present_HP += GameManager.GetComponent<GameManager>().Status[0];
        GameManager.GetComponent<GameManager>().Present_MP += GameManager.GetComponent<GameManager>().Status[1];

        GameManager.GetComponent<GameManager>().HP += GameManager.GetComponent<GameManager>().Status[0];
        GameManager.GetComponent<GameManager>().MP += GameManager.GetComponent<GameManager>().Status[1];
        GameManager.GetComponent<GameManager>().AttackStatus += GameManager.GetComponent<GameManager>().Status[2];
        GameManager.GetComponent<GameManager>().DefenseStatus += GameManager.GetComponent<GameManager>().Status[3];
        GameManager.GetComponent<GameManager>().job = job;
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
            HP = Random.Range(HP_slot_Min, HP_slot_Max + 1);
            MP = Random.Range(MP_slot_Min, MP_slot_Max + 1);
            Attack = Random.Range(power_slot_Min, power_slot_Max + 1);
            Defense = Random.Range(Defense_slot_Min, Defense_slot_Max + 1);

            if (FirstRoulette)
                job = Random.Range(0, 3);

            UpdateStatus = new int[] { HP, MP, Attack, Defense };

            UpdateStatusText();
            yield return new WaitForSeconds(0.05f);
        }

        void UpdateStatusText()
        {
            HP_text.text = "HP：" + Mathf.RoundToInt(HP);
            MP_text.text = "MP：" + Mathf.RoundToInt(MP);
            Attack_text.text = "攻撃：" + Mathf.RoundToInt(Attack);
            defense_text.text = "防御：" + Mathf.RoundToInt(Defense);
        }
    }
}

