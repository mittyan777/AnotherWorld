using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [Header("設定")]
    public Transform warpTarget; // ワープ先

    // ↓ここが追加：あなたのプレイヤーを動かしているスクリプトの名前を書いてください
    // （例："PlayerController", "ThirdPersonController", "StarterAssetsInputs" など）
    [Tooltip("プレイヤーの移動制御スクリプトの名前を正確に入力してください")]
    public string movementScriptName = "PlayerMovementttttt";

    private const string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            StartCoroutine(WarpAfterDelay(other));
        }
    }

    IEnumerator WarpAfterDelay(Collider player)
    {
        // 1. 移動スクリプトを探して、一時的に「オフ」にする
        // stringで指定した名前のスクリプトコンポーネントを取得します
        MonoBehaviour moveScript = player.GetComponent(movementScriptName) as MonoBehaviour;

        if (moveScript != null)
        {
            moveScript.enabled = false; // 操作不能にする
        }
        else
        {
            Debug.LogWarning("移動スクリプトが見つかりません。Inspectorで名前が合っているか確認してください。");
        }

        // ついでにRigidbodyの勢い（慣性）も止める（滑り防止）
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 2. 1秒待つ（この間、プレイヤーは動けない）
        yield return new WaitForSeconds(2.0f);

        // 3. ワープさせる
        if (warpTarget != null && player != null)
        {
            player.transform.position = warpTarget.position;
            Debug.Log("1秒経過：ワープしました！");
        }

        // 4. 移動スクリプトを「オン」に戻す
        if (moveScript != null)
        {
            moveScript.enabled = true; // 操作可能に戻す
        }
    }
}
