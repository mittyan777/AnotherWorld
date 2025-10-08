using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouretteManejer : MonoBehaviour
{
    public GameObject rouletteUI;   // UI
    public NewPlayer player;        // プレイヤー操作スクリプト

    private bool isInRange = false; // 範囲内判定
    private bool isActive = false;  // UIが開いてるか

    void Update()
    {
        if (isInRange && !isActive && Input.GetKeyDown(KeyCode.E))//もしも「Eキー」を押した場合（Open...)を呼び起こす
        {
            OpenRoulette();
        }
    }

    void OpenRoulette()
    {
        isActive = true;
        player.canControl = false;        // 操作停止
        rouletteUI.SetActive(true);       // UIを開く

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseRoulette()
    {
        isActive = false;
        player.canControl = true;         // 操作再開
        rouletteUI.SetActive(false);      // UIを閉じる

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRange = false;
    }

}
