using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollViewController : MonoBehaviour
{
    [Header("Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    [Header("ScrollViews")]
    public GameObject scrollView1;
    public GameObject scrollView2;
    public GameObject scrollView3;
    public GameObject scrollView4;

    // ボタンが表示されているかどうかを管理
    private bool buttonsVisible = false;

    void Start()
    {
        // 各ボタンのクリックイベント設定
        button1.onClick.AddListener(() => ShowScrollView(1));
        button2.onClick.AddListener(() => ShowScrollView(2));
        button3.onClick.AddListener(() => ShowScrollView(3));
        button4.onClick.AddListener(() => ShowScrollView(4));

        // 起動時は全て非表示
        HideAllScrollViews();
        HideAllButtons();
    }

    void Update()
    {
        // Bキーで表示・非表示を切り替え
        if (Input.GetKeyDown(KeyCode.B))
        {
            buttonsVisible = !buttonsVisible;

            if (buttonsVisible)
            {
                ShowAllButtons();
            }
            else
            {
                HideAllButtons();
                HideAllScrollViews();
            }
        }
    }

    private void HideAllScrollViews()
    {
        scrollView1.SetActive(false);
        scrollView2.SetActive(false);
        scrollView3.SetActive(false);
        //scrollView4.SetActive(false);
    }

     private void HideAllButtons()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
    }

    private void ShowScrollView(int index)
    {
        // まず全て非表示
        HideAllScrollViews();

        // 指定した番号のScrollViewを表示
        switch (index)
        {
            case 1: scrollView1.SetActive(true); break;
            case 2: scrollView2.SetActive(true); break;
            case 3: scrollView3.SetActive(true); break;
            //case 4: scrollView4.SetActive(true); break;
        }
    }
      private void ShowAllButtons()
    {
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);
        //button4.gameObject.SetActive(true);
    }   
}