using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloatManejert : MonoBehaviour
{
    [Header("MenuManejer �������Ƀh���b�O")]
    public MenuManejer menuManager;

    void OnEnable()
    {
        // �X���b�g��ʂ��L���ɂȂ����u�Ԃ� MenuManejer ���~
        if (menuManager != null)
        {
            menuManager.enabled = false;
            Debug.Log("�X���b�g���[�h���FMenuManejer������");
        }
    }

    void OnDisable()
    {
        // �X���b�g��ʂ�����ꂽ�� MenuManejer ���ĂїL����
        if (menuManager != null)
        {
            menuManager.enabled = true;
            Debug.Log("�X���b�g���[�h�I���FMenuManejer�ėL����");
        }
    }

}
