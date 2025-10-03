using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RurettoPlate : MonoBehaviour
{
    public GameObject rouletteUI; // ルーレットUI
    public Player_Mouri player; // プレイヤー操作スクリプト
    private bool isInRange = false;
    private bool isActive = false;

    void Update()
    {
        if (isInRange && !isActive && Input.GetKeyDown(KeyCode.E))
        {
            StartRoulette();
        }
    }

    void StartRoulette()
    {
        isActive = true;
        player.enabled = false; // プレイヤー移動無効
        rouletteUI.SetActive(true); // ルーレット画面ON
    }

    public void EndRoulette()
    {
        isActive = false;
        player.enabled = true; // プレイヤー移動有効
        rouletteUI.SetActive(false); // ルーレット画面OFF
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isInRange = false;
    }
}
