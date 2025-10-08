using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloatManejert : MonoBehaviour
{
    [Header("MenuManejer をここにドラッグ")]
    public MenuManejer menuManager;

    void OnEnable()
    {
        // スロット画面が有効になった瞬間に MenuManejer を停止
        if (menuManager != null)
        {
            menuManager.enabled = false;
            Debug.Log("スロットモード中：MenuManejer無効化");
        }
    }

    void OnDisable()
    {
        // スロット画面が閉じられたら MenuManejer を再び有効化
        if (menuManager != null)
        {
            menuManager.enabled = true;
            Debug.Log("スロットモード終了：MenuManejer再有効化");
        }
    }

}
