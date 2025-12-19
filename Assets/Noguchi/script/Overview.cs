using UnityEngine;

public class Overview : MonoBehaviour
{
    [Header("順番に表示するCanvas")]
    public GameObject[] canvases;

    private int currentIndex = 0;
    private bool isOpen = false;

    void Start()
    {
        CloseAll();
    }

    void Update()
    {
        // Escキーで開閉
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen){CloseAll();}
            else{Open();}
        }

        if (!isOpen) return;

        // 右キー（次へ）
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {ShowNext();}

        // 左キー（前へ）
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {ShowPrevious();}
    }

    void Open()
    {
        isOpen = true;
        currentIndex = 0;
        ShowCurrent();
    }

    void CloseAll()
    {
        isOpen = false;
        foreach (var canvas in canvases)
        { canvas.SetActive(false);}
    }

    void ShowCurrent()
    {
        CloseAll();
        isOpen = true;
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
