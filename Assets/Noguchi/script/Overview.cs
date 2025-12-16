using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
   [Header("順番に表示するCanvas")]
    public GameObject[] canvases;

    [Header("遷移先シーン名")]
    public string nextSceneName;
    private int currentIndex = 0;
    private bool isFinished = false;

    void Start()
    {
        // 全Canvasを非表示
        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }

        // 最初のCanvasを表示
        if (canvases.Length > 0)
        {
            canvases[0].SetActive(true);
        }
    }

    void Update()
    {
        if (isFinished) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowNextCanvas();
        }
    }

    void ShowNextCanvas()
    {
        // 最後のCanvasならシーン遷移
        if (currentIndex >= canvases.Length - 1)
        {
            isFinished = true;
            LoadNextScene();
            return;
        }
        canvases[currentIndex].SetActive(false);
        currentIndex++;
        canvases[currentIndex].SetActive(true);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
