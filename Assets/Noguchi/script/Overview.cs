using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   [Header("順番に表示するCanvas")]
    public GameObject[] canvases;

    private int currentIndex = 0;

    void Start()
    {
        // 全Canvasを非表示
        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }

        // 最初のCanvasを表示
        if (canvases.Length > 0)
        {
            canvases[0].SetActive(true);
        }
    }

    void Update()
    {
        // Enterキーで次へ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowNextCanvas();
        }
    }

    void ShowNextCanvas()
    {
        // 現在のCanvasを非表示
        canvases[currentIndex].SetActive(false);

        currentIndex++;

        // 配列範囲チェック
        if (currentIndex >= canvases.Length)
        {
            currentIndex = canvases.Length - 1;
            return;
        }
        
         // 次のCanvasを表示
        canvases[currentIndex].SetActive(true);
    }
}
