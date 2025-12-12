using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rouletto_CG : MonoBehaviour
{
    [SerializeField] private Image targetImage; // 職種画像を表示するImage

    [Header("職種ごとの画像")]
    [SerializeField] private Sprite swordsman;
    [SerializeField] private Sprite Magishan;
    [SerializeField] private Sprite Aceher;

    private bool isFirstRoulette = true;

    [Header("コインのフェードアウト")]
    [SerializeField] private GameObject Fadeout_Coin;
    [SerializeField] private Text CoinText;
    [SerializeField] private float fadeTime;
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = new Color(1f, 1f, 1f, 0f); // 透明にフェードアウト

    private GameManager gameManager;

    // ★ 追加：高速で画像切り替え用のフラグ
    private bool isSpinning = false;

    // ★ 追加：コルーチン管理用（コインフェードが重複しないように）
    private Coroutine coinFadeCoroutine;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // ★ 追加：画像スロット（順番に回す）
    private Sprite[] spinSprites => new Sprite[] { swordsman, Magishan, Aceher };

    // ★ 追加：ルーレット演出スタート
    public void StartSpinEffect(float interval = 0.1f)
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinImage(interval));
        }
    }

    // ★ 追加：画像を高速で切り替えて回転演出
    private IEnumerator SpinImage(float interval)
    {
        isSpinning = true;
        int index = 0;

        while (isSpinning)
        {
            targetImage.sprite = spinSprites[index];
            index = (index + 1) % spinSprites.Length;
            yield return new WaitForSeconds(interval);
        }
    }

    // ★ 追加：ルーレット終了し本来の画像を表示
    public void StopSpinAndShow(string jobName)
    {
        isSpinning = false; // 回転停止
        ShowJob(jobName);   // 結果画像を表示
    }

    // 初回ルーレット時に呼ぶ
    public void ShowJob(string jobName)
    {
        if (targetImage == null) return;

        if (jobName == "剣士")
            targetImage.sprite = swordsman;
        else if (jobName == "弓使い")
            targetImage.sprite = Magishan;
        else if (jobName == "魔法使い")
            targetImage.sprite = Aceher;

        targetImage.gameObject.SetActive(true);
        isFirstRoulette = false;

        Debug.Log("初回ルーレットで表示する職業: " + jobName);
    }

    // 二回目以降は職種画像を非表示にする
    public void HideJobImage()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(false);
            Debug.Log("2回目以降は職業画像非表示");
        }
    }

    // Rouletto_New から呼ぶ共通処理
    public void UpdateJobImage(string jobName, bool firstRoulette)
    {
        if (firstRoulette)
            ShowJob(jobName);
        else
            HideJobImage();
    }

    // コイン使用＆フェードアウト
    public void UseCoin(int coin)
    {
        if (Fadeout_Coin == null || CoinText == null) return;
        if (gameManager.Coin < coin) return;

        Fadeout_Coin.SetActive(true);

        CoinText.text = "-" + coin.ToString();
        CoinText.color = new Color(1f, 1f, 1f, 1f); // 不透明に初期化

        // 既存コルーチンが動作中なら停止
        if (coinFadeCoroutine != null)
            StopCoroutine(coinFadeCoroutine);

        coinFadeCoroutine = StartCoroutine(CoinFadeOut());
    }

    private IEnumerator CoinFadeOut()
    {
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeTime;
            CoinText.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        Fadeout_Coin.SetActive(false);
        CoinText.color = startColor;
        coinFadeCoroutine = null;
    }
}
