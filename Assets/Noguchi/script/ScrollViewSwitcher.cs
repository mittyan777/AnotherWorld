using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollViewSwitcher : MonoBehaviour
{
    // UnityエディタでScroll Viewオブジェクトをアサインするためのリスト
    [SerializeField]
    private List<GameObject> scrollViews = new List<GameObject>();

    // 現在表示されているScroll Viewのインデックス
    private int currentIndex = 0;

    void Start()
    {
        // 初期設定: すべてのScroll Viewを非表示にし、最初のものだけを表示する
        InitializeScrollViews();
    }

    // すべてのScroll Viewを非表示にし、最初のScroll Viewを表示する
    private void InitializeScrollViews()
    {
        if (scrollViews.Count == 0)
        {
            Debug.LogError("Scroll Viewsが設定されていません。エディタでリストにオブジェクトをドラッグ＆ドロップしてください。");
            return;
        }

        // 全て非アクティブにする
        foreach (GameObject scrollView in scrollViews)
        {
            if (scrollView != null)
            {
                scrollView.SetActive(false);
            }
        }

        // 最初のScroll Viewをアクティブにする
        if (scrollViews[0] != null)
        {
            scrollViews[0].SetActive(true);
            currentIndex = 0; // インデックスをリセット
        }
    }

    // ButtonのOnClickイベントに設定するメソッド
    public void SwitchScrollView()
    {
        if (scrollViews.Count == 0) return;

        // 1. 現在のScroll Viewを非表示にする
        if (scrollViews[currentIndex] != null)
        {
            scrollViews[currentIndex].SetActive(false);
        }

        // 2. インデックスを更新する (0 -> 1 -> 2 -> 3 -> 0 のように循環させる)
        currentIndex = (currentIndex + 1) % scrollViews.Count;

        // 3. 次のScroll Viewを表示する
        if (scrollViews[currentIndex] != null)
        {
            scrollViews[currentIndex].SetActive(true);
            
            // ログで現在の表示を確認
            Debug.Log($"Scroll Viewを切り替えました。現在表示: {scrollViews[currentIndex].name}");
        }
    }
}