using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteUIManager : MonoBehaviour
{
    public static RouletteUIManager Instance;
    public GameObject rouletteUI;

    private NewPlayer currentPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���[���b�gUI���J��
    public void OpenRouletteUI(NewPlayer player)
    {
        currentPlayer = player;
        rouletteUI.SetActive(true);

        player.canControl = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ���[���b�gUI�����iUI�̕���{�^������Ăԁj
    public void CloseRouletteUI()
    {
        if (currentPlayer != null)
        {
            currentPlayer.canControl = true;
        }

        rouletteUI.SetActive(false);
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //
    }
}
