using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouretteManejer : MonoBehaviour
{
    public GameObject rouletteUI;   // UI
    public NewPlayer player;        // �v���C���[����X�N���v�g

    private bool isInRange = false; // �͈͓�����
    private bool isActive = false;  // UI���J���Ă邩

    void Update()
    {
        if (isInRange && !isActive && Input.GetKeyDown(KeyCode.E))//�������uE�L�[�v���������ꍇ�iOpen...)���ĂыN����
        {
            OpenRoulette();
        }
    }

    void OpenRoulette()
    {
        isActive = true;
        player.canControl = false;        // �����~
        rouletteUI.SetActive(true);       // UI���J��

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseRoulette()
    {
        isActive = false;
        player.canControl = true;         // ����ĊJ
        rouletteUI.SetActive(false);      // UI�����

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
