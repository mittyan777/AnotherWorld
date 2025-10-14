using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTPSController : MonoBehaviour
{
    // インスペクターで設定する項目
    [Header("ターゲット設定")]
    [Tooltip("カメラが追従し、中心となって回転する対象（プレイヤー）")]
    public Transform targetplayer;

    [Header("カメラ設定")]
    [Tooltip("ターゲットからの距離")]
    public float distance = 5.0f;
    [Tooltip("垂直方向の視点制限（最小角度）")]
    public float minYAngle = -10.0f;
    [Tooltip("垂直方向の視点制限（最大角度）")]
    public float maxYAngle = 80.0f;

    [Header("感度設定")]
    [Tooltip("水平方向の感度")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("垂直方向の感度")]
    public float mouseSensitivityY = 2.0f;

    private float currentX = 0.0f; // 水平方向の累積回転角度
    private float currentY = 0.0f; // 垂直方向の累積回転角度

    void Start()
    {
        // カーソルをロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウスの移動量を取得
        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY; // Y軸は上下反転させるためマイナス

        // 垂直方向の回転角度を制限
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (targetplayer == null) return;

        // 1. 回転を計算
        // 累積した角度からカメラの回転（Quaternion）を作成
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 2. 位置を計算
        // ターゲットの位置から、回転を考慮しつつ指定距離だけ離れた位置を計算
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + targetplayer.position;

        // 3. カメラに適用
        transform.rotation = rotation;
        transform.position = position;
    }
}
