using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpShow : MonoBehaviour
{
    [Header("ここに表示させたい別のオブジェクトを入れる")]
    public GameObject targetObject;

    // 「もう発動したかどうか」を記録するスイッチ
    private bool hasActivated = false;

    // 触れた瞬間に実行される
    void OnTriggerEnter(Collider other)
    {
        // もし「すでに発動済み」なら、ここで処理を終了する（何もしない）
        if (hasActivated == true)
        {
            return;
        }

        // ここから下は「まだ発動していない」時だけ通る

        hasActivated = true;  // 「発動済み」に書き換える

        targetObject.SetActive(true); // 別のオブジェクトを表示！
        Invoke("HideObject", 2.0f);   // 2秒後に「HideObject」を実行
    }

    // 消す処理
    void HideObject()
    {
        targetObject.SetActive(false); // 別のオブジェクトを消す

        // 【補足】
        // もし「消した後に、もう一度だけ復活させたい」場合は
        // ここで hasActivated = false; に戻せばOKです。
        // 今回は「二度と表示しない」ので、このままで大丈夫です。
    }
}
