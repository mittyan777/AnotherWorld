using UnityEngine;
using UnityEngine.SceneManagement;
public class choice : MonoBehaviour
{
    [Header("このボタンの遷移先シーン名")]
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("遷移先シーン名が設定されていません", this);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
