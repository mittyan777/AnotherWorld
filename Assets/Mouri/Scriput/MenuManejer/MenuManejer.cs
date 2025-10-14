using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class MenuManejer : MonoBehaviour
{
    //���j���[�̃��[�g�I�u�W�F�N�g(�S�̂��܂Ƃ߂� Canvas �Ȃ�)
    public GameObject menuUI;

    // ������Ԃ��\���ɂ��邩�ǂ���
    public bool startMenuHidden = true;

    // �|�[�Y�����ǂ������Ǘ�
    public bool isPaused = false;

    public AudioSource Audio;//�ǉ�

    public AudioSource Audio2;



    void Start()
    {
        // ������ԂŃ��j���[���\���ɂ���
        if (menuUI != null)
        {
            menuUI.SetActive(!startMenuHidden);
        }
        if (startMenuHidden)
        {
            ResumeGame();
        }
    }

    void Update()
    {
        // Esc�L�[�Ń��j���[��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();

                PauseAudio();//�ǉ�
            }
            else
            {
                PauseGame();

                PauseAudio();//�ǉ�
            }

        }


    }

    public void PauseGame()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(true); // ���j���[��\��
        }
        Time.timeScale = 0f; // �Q�[�����~
        isPaused = true;
    }
    public void PauseAudio()  //PauseAudio�̒��̈Ӗ�//�ǉ�
    {
        if (Audio != null)//������
        {
            Audio.Play();�@//�ǉ�
        }
        if (isPaused)//�ǉ�
        {
            Audio.Pause();//�ǉ�
        }



    }
    
    public void ResumeGame()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(false); // ���j���[���\��
        }
        Time.timeScale = 1f; // �Q�[�����ĊJ
        isPaused = false;
    }
}
