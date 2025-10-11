using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class MenuManejer : MonoBehaviour
{
    //メニューのルートオブジェクト(全体をまとめた Canvas など)
    public GameObject menuUI;

    // 初期状態を非表示にするかどうか
    public bool startMenuHidden = true;

    // ポーズ中かどうかを管理
    public bool isPaused = false;

    public AudioSource Audio;//追加

    public AudioSource Audio2;



    void Start()
    {
        // 初期状態でメニューを非表示にする
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
        // Escキーでメニューを切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();

                PauseAudio();//追加
            }
            else
            {
                PauseGame();

                PauseAudio();//追加
            }

        }


    }

    public void PauseGame()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(true); // メニューを表示
        }
        Time.timeScale = 0f; // ゲームを停止
        isPaused = true;
    }
    public void PauseAudio()  //PauseAudioの中の意味//追加
    {
        if (Audio != null)//もしも
        {
            Audio.Play();　//追加
        }
        if (isPaused)//追加
        {
            Audio.Pause();//追加
        }



    }
    
    public void ResumeGame()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(false); // メニューを非表示
        }
        Time.timeScale = 1f; // ゲームを再開
        isPaused = false;
    }
}
