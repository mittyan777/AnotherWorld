using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menucs : MonoBehaviour
{
   [Header("メニューUI")]
    public GameObject menuCanvas;

    [Header("概要UI")]
    public Overview overview;

    private bool isMenuOpen = false;
    private bool isOverviewOpen = false;

    void Start()
    {
        menuCanvas.SetActive(false);
        overview.Close();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 概要表示中 → メニューに戻る
            if (isOverviewOpen)
            {
                CloseOverview();
                OpenMenu();
            }
            // メニュー表示中 → 閉じる
            else if (isMenuOpen)
            {
                CloseMenu();
            }
            // 何も表示していない → メニューを開く
            else
            {
                OpenMenu();
            }
        }
    }

    // ===== Buttonから呼ぶ =====
    public void OnOverviewButton()
    {
        CloseMenu();
        OpenOverview();
    }

    public void OverViewButton() 
    {
        OpenOverview();
    }

    // ===== メニュー制御 =====
    void OpenMenu()
    {
        isMenuOpen = true;
        menuCanvas.SetActive(true);
    }

    void CloseMenu()
    {
        isMenuOpen = false;
        menuCanvas.SetActive(false);
    }

    // ===== 概要制御 =====
    void OpenOverview()
    {
        isOverviewOpen = true;
        overview.Open();
    }

    void CloseOverview()
    {
        isOverviewOpen = false;
        overview.Close();
    }

    
}
