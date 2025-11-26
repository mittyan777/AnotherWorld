using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class END : MonoBehaviour
{
    [Header("フェード対象のText（4つ）")]
    public Text[] texts = new Text[4];

    [Header("フェード時間（秒）")]
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;

    [Header("フェードイン後の待ち時間（秒）")]
    public float waitTime = 2.0f;

    [Header("タイトルシーン名")]
    public string titleSceneName = "Title";

    private void Start()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            Text txt = texts[i];

            if (txt == null)
            {
                Debug.LogWarning($"texts[{i}] が設定されていません");
                continue;
            }

            // ▼▼ ここが重要！非表示なら表示状態にしておく ▼▼
            txt.gameObject.SetActive(true);

            // アルファ0の透明状態で開始
            Color c = txt.color;
            c.a = 0f;
            txt.color = c;

            // フェードイン
            yield return StartCoroutine(FadeText(txt, 0f, 1f, fadeInTime));

            // 表示したまま待機
            yield return new WaitForSeconds(waitTime);

            // フェードアウト
            yield return StartCoroutine(FadeText(txt, 1f, 0f, fadeOutTime));

            // フェードアウト後に非表示にしておく（次のテキストと重ならないように）
            txt.gameObject.SetActive(false);
        }

        // すべて終わったらタイトルへ
        SceneManager.LoadScene(titleSceneName);
    }

    private IEnumerator FadeText(Text target, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color c = target.color;

        while (time < duration)
        {
            float t = time / duration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            target.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        // 最終値固定
        c.a = endAlpha;
        target.color = c;
    }
}
