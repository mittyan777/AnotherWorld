using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloatManejert : MonoBehaviour
{
    //���̃��j���[���Ǘ����Ă���X�N���v�g
    public MenuManejer menuManager;

    //���̃X�N���v�g�������Ă���I�u�W�F�N�g��

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
