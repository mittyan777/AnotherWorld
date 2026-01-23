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
    [SerializeField] private Color endColor = Color.white;

    [Header("コイン不足時のUI")]
    [SerializeField] public GameObject no_Coins;
    [SerializeField] public Text no_CoinText;
    [SerializeField] public float no_CoinTime = 1.0f;
    [SerializeField] public float display_Time = 0.5f;


    private bool islackPlaying;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    // 初回ルーレット時に呼ぶ
    public void ShowJob(string jobName)
    {
        if (targetImage == null) return;

        if (jobName == "剣士")
        {
            targetImage.sprite = swordsman;
        }
        else if (jobName == "弓使い")
        {
            targetImage.sprite = Magishan;
        }
        else if (jobName == "魔法使い")
        {
            targetImage.sprite = Aceher;
        }

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
        {
            ShowJob(jobName);
        }
        else
        {
            HideJobImage();
        }
    }
    public void UseCoin(int coin)   //コイン消費
    {
        if (Fadeout_Coin == null || CoinText == null) return;

        if (gameManager.Coin < coin)
        {
            Coin_lack();
            return;
        }

        Fadeout_Coin.SetActive(true);

        //アルファを初期化（透明な部分）
        Color color = CoinText.color;
        CoinText.color = new Color(color.r, color.g, color.b, 1f);
        CoinText.text = "-" + coin.ToString();

        StartCoroutine(CoinFadeOut());
    }


    private IEnumerator CoinFadeOut()
    {
        Color start = startColor;
        Color end = endColor;

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float FadeTime = timer / fadeTime;

            CoinText.color = Color.Lerp(start, end, FadeTime);

            yield return null;
        }

        Fadeout_Coin.SetActive(false);

        //色を元に戻す
        CoinText.color = start;

    }
    public void Coin_lack()
    {

        if (islackPlaying) return;

        if(no_Coins==null||no_CoinText==null)return;

        StopAllCoroutines();
        no_Coins.SetActive(true);
        StartCoroutine(LackCoinFade());
    }

    //コインを不足したとき
    private IEnumerator LackCoinFade()
    {
        islackPlaying = true;

        //透過リセット
        Color color = no_CoinText.color;
        no_CoinText.color = new Color(color.r, color.g, color.b, 1f);

        yield return new WaitForSeconds(display_Time);

        float timer = 0f;

        while(timer < no_CoinTime)
        {
            timer += Time.deltaTime;

            float t=timer / no_CoinTime;

            Color newColor = no_CoinText.color;
            newColor.a = Mathf.Lerp(1f,0f,t);
            no_CoinText.color=newColor;

            yield return null;
        }

        //フェードが終わった後、非表示
        no_Coins.SetActive(false);
        islackPlaying=false;


    }
}
