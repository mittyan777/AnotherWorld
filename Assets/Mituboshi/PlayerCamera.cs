using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 currentPos; // 現在のカメラ位置
    Vector3 pastPos;    // 過去のカメラ位置
    Vector3 diff;       // 移動距離

    // カメラ回転用
    float verticalAngle = 0f; // 縦回転角度を保持（自分で管理する）
    [SerializeField] float minVertical = -30f; // 下を向ける最小角度
    [SerializeField] float maxVertical = 30f;  // 上を向ける最大角度
    [SerializeField] float sensitivity = 1f;   // マウス感度

    private void Start()
    {
        // 最初のプレイヤーの位置を記録
        pastPos = player.transform.position;
    }

    void Update()
    {
        // ------ カメラの移動 ------
        currentPos = player.transform.position;
        diff = currentPos - pastPos;
        transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);
        pastPos = currentPos;

        // ------ カメラの回転 ------
        float mx = Input.GetAxis("Mouse X") * sensitivity;
        float my = Input.GetAxis("Mouse Y") * sensitivity;

        // 横回転（プレイヤーの周囲を回転）
        if (Mathf.Abs(mx) > 0.01f)
        {
            transform.RotateAround(player.transform.position, Vector3.up, mx);
        }

        // 縦回転（角度制限あり）
        if (Mathf.Abs(my) > 0.01f)
        {
            verticalAngle += -my; // 上下は逆なのでマイナスを付ける
            verticalAngle = Mathf.Clamp(verticalAngle, minVertical, maxVertical);

            // 現在のカメラの角度から、制御したいX軸回転だけ設定
            Vector3 euler = transform.eulerAngles;
            euler.x = verticalAngle;
            transform.eulerAngles = new Vector3(euler.x, transform.eulerAngles.y, 0);
        }

        // プレイヤーの向きはカメラのYだけ反映
        player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
