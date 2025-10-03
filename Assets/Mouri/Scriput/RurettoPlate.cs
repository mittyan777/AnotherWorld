using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RurettoPlate : MonoBehaviour
{
    public GameObject rouletteUI; // ���[���b�gUI
    public Player_Mouri player; // �v���C���[����X�N���v�g
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
        player.enabled = false; // �v���C���[�ړ�����
        rouletteUI.SetActive(true); // ���[���b�g���ON
    }

    public void EndRoulette()
    {
        isActive = false;
        player.enabled = true; // �v���C���[�ړ��L��
        rouletteUI.SetActive(false); // ���[���b�g���OFF
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
