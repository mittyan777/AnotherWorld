using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloatManejar : MonoBehaviour
{
    [SerializeField] NewPlayer player;

    // ����͐��������܂����u�ԂɌĂ΂��z��
    public void OnSlotFinished()
    {
        Debug.Log("�X���b�g�I���IUI����đ���ĊJ");
        player.Slotstop();
    }
}