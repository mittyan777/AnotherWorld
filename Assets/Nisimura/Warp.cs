using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    // Inspectorでワープ先のTransformを設定します。
    // Unityのシーン上で、ワープさせたい目標地点のオブジェクトをドラッグ＆ドロップで設定します。
    public Transform warpTarget;

    // ワープさせる対象をタグで識別します。
    // ここでは "Player" タグを持つオブジェクトが対象であることを想定しています。
    private const string PlayerTag = "Player";

    // 衝突（Collider）を検出したときに呼び出されるメソッド
    private void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトがプレイヤーかどうか、タグで確認
        if (other.CompareTag(PlayerTag))
        {
            // ワープ先のTransformが設定されているか確認
            if (warpTarget != null)
            {
                // プレイヤーの位置をワープ先の位置に瞬間移動させる
                other.transform.position = warpTarget.position;

                Debug.Log(other.gameObject.name + "をワープさせました！");
            }
            else
            {
                Debug.LogError("Warp Targetが設定されていません。Inspectorで設定してください。");
            }
        }
    }
}
