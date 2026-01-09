using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
     [Header("ワープ設定")]
    [SerializeField] private Transform player;      // 移動させたいプレイヤー
    [SerializeField] private Transform warpPoint;   // ワープ先

    [Header("シーン遷移")]
    [SerializeField] private string titleSceneName; // タイトルシーン名

    // Button1：ワープ
    public void WarpPlayer()
    {
        if (player == null || warpPoint == null)
        {
            Debug.LogWarning("Player または WarpPoint が設定されていません");
            return;
        }

        player.position = warpPoint.position;
    }

    // Button2：タイトルへ
    public void GoToTitle()
    {
        if (string.IsNullOrEmpty(titleSceneName))
        {
            Debug.LogWarning("タイトルシーン名が設定されていません");
            return;
        }

        SceneManager.LoadScene(titleSceneName);
    }
}
