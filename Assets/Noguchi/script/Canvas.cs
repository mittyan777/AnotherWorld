using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;

    void Start()
    {
         // ボタンAの判定
        if (buttonA != null)
        {
            buttonA.onClick.AddListener(() => OnButtonClicked("A"));
            Debug.Log("ボタンAの監視を開始しました。");
        }
        else
        {
            Debug.LogError("buttonA が設定されていません！");
        }

        // ボタンBの判定
        if (buttonB != null)
        {
            buttonB.onClick.AddListener(() => OnButtonClicked("B"));
            Debug.Log("ボタンBの監視を開始しました。");
        }
        else
        {
            Debug.LogError("buttonB が設定されていません！");
        }
    }

    // ボタンが押された時に呼ばれる処理
    void OnButtonClicked(string buttonName)
    {
        Debug.Log($"ボタン{buttonName} が押されました！");
    }

    void Update()
    {
        // 押せない状態（interactable = false）のチェック
        if (buttonA != null && !buttonA.interactable)
        {
            Debug.Log("ボタンA は無効状態で押せません。");
        }

        if (buttonB != null && !buttonB.interactable)
        {
            Debug.Log("ボタンB は無効状態で押せません。");
        }
    }
}
