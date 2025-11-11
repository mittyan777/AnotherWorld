using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Image fadeImage; // 黒いフェード用イメージ
     [SerializeField] private string SceneName = "Scene";

    private bool isFading = false;

    void Start()
    {
        // ボタンA
        if (buttonA != null)
        {   buttonA.onClick.AddListener(() => OnButtonClicked("A"));
            Debug.Log("ボタンAの監視を開始しました。");}
        else
        {Debug.LogError("buttonA が設定されていません！");}

        // ボタンB
        if (buttonB != null)
        {   buttonB.onClick.AddListener(() => OnButtonClicked("B"));
            Debug.Log("ボタンBの監視を開始しました。");}
        else
        {Debug.LogError("buttonB が設定されていません！");}

        // フェードイメージ初期化
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f; // 完全に透明からスタート
            fadeImage.color = c;
        }
    }

    void OnButtonClicked(string buttonName)
    {
        Debug.Log($"ボタン{buttonName} が押されました！");

        if (buttonName == "B" && !isFading)
        {StartCoroutine(FadeOutAndQuit());}
    }

    private IEnumerator FadeOutAndQuit()
    {
        isFading = true;

        float duration = 2f; // フェード時間
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / duration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }

            yield return null;
        }

        Debug.Log($"フェードアウト完了。シーン「{SceneName}」へ移動します。");
        SceneManager.LoadScene(SceneName);

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ実行停止
        #else
        Application.Quit(); // ビルド後の実行ファイル終了
        #endif
    }

    void Update()
    {
        if (buttonA != null && !buttonA.interactable)
           {Debug.Log("ボタンA は無効状態で押せません。");}

        if (buttonB != null && !buttonB.interactable)
           {Debug.Log("ボタンB は無効状態で押せません。");}
    }
}
