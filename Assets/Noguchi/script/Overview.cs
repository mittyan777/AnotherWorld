using UnityEngine;

public class Overview : MonoBehaviour
{
    [Header("順番に表示するPanel(子オブジェクト)")]
    [SerializeField] private GameObject[] panels;

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

    // ===== 外部操作 =====
    public void Open()
    {
        if (panels == null || panels.Length == 0) return;

        isOpen = true;
        currentIndex = 0;
        ShowCurrent();
    }

    public void Close()
    {
        isOpen = false;

        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    // ===== 内部処理 =====
    private void ShowCurrent()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentIndex);
        }
    }

    private void ShowNext()
    {
        if (currentIndex >= panels.Length - 1) return;

        currentIndex++;
        ShowCurrent();
    }

    private void ShowPrevious()
    {
        if (currentIndex <= 0) return;

        currentIndex--;
        ShowCurrent();
    }
}
