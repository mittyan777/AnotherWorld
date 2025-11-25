using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Changimage : MonoBehaviour
{

    [SerializeField] private Image targetImage; // 職種画像を表示するImage

    [Header("職種ごとの画像")]
    [SerializeField] private Sprite swordsman;
    [SerializeField] private Sprite Magishan;
    [SerializeField] private Sprite Aceher;

    private bool isFirstRoulette = true;

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
}