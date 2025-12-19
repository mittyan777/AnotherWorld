using UnityEngine;

public class Overview : MonoBehaviour
{
   [Header("順番に表示するCanvas")]
    public GameObject[] canvases;

    private int currentIndex = 0;
    private bool isOpen = false;

    void Start()
    {
        Close();
    }

    void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowNext();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShowPrevious();
        }
    }

    // ===== 外部操作用 =====
    public void Open()
    {
        isOpen = true;
        currentIndex = 0;
        ShowCurrent();
    }

    public void Close()
    {
        isOpen = false;
        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }
    }

    // ===== 内部処理 =====
    void ShowCurrent()
    {
        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }
        canvases[currentIndex].SetActive(true);
    }

    void ShowNext()
    {
        if (currentIndex >= canvases.Length - 1) return;
        currentIndex++;
        ShowCurrent();
    }

    void ShowPrevious()
    {
        if (currentIndex <= 0) return;
        currentIndex--;
        ShowCurrent();
    }
}
