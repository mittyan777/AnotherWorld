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


    // 初回ルーレット時に呼ぶ
    public void ShowJob(string jobName)
    {
        if (targetImage == null) return;

        if (jobName == "剣士")
        {
            targetImage.sprite = swordsman;
        }
        else if (jobName == "魔法使い")
        {
            targetImage.sprite = Magishan;
        }
        else if (jobName == "弓使い")
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
    public void UseCoin(int coin)   //初回コイン消費
    {
        if (Fadeout_Coin == null || CoinText == null) return;

        Fadeout_Coin.SetActive(true);

        //アルファを初期化（透明な部分）
        Color color = CoinText.color;
        CoinText.color = new Color(color.r, color.g, color.b, 1f);
        CoinText.text = "-" + coin.ToString();

        StartCoroutine(CoinFadeOut());
    }


    private IEnumerator CoinFadeOut()
    {
        Color startColor = CoinText.color;
        Color endColor = new Color(this.startColor.r, this.startColor.g, this.startColor.b, 0f);

        float timern = 0f;

        while (timern < fadeTime)
        {
            timern += Time.deltaTime;
            float FadeTime = timern / fadeTime;

            CoinText.color = Color.Lerp(startColor, this.endColor, FadeTime);

            yield return null;
        }

        Fadeout_Coin.SetActive(false);

        //色を元に戻す
        CoinText.color = new Color(startColor.r, startColor.g, startColor.b, 1f);


    }
}
