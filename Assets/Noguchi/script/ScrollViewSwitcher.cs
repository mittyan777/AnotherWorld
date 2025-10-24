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

    void Start()
    {
        // 各ボタンのクリックイベント設定
        button1.onClick.AddListener(() => ShowScrollView(1));
        button2.onClick.AddListener(() => ShowScrollView(2));
        button3.onClick.AddListener(() => ShowScrollView(3));
        button4.onClick.AddListener(() => ShowScrollView(4));

        // 起動時は全て非表示
        HideAllScrollViews();
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
            case 4: scrollView4.SetActive(true); break;
        }
    }

    private void HideAllScrollViews()
    {
        scrollView1.SetActive(true);
        scrollView2.SetActive(false);
        scrollView3.SetActive(false);
        scrollView4.SetActive(false);
    }
}