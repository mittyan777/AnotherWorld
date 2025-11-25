using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCompomentttttttt : MonoBehaviour
{
    [Header("ターゲット")]
    public Transform target; // 追従するプレイヤーキャラクターのTransform

    [Header("設定")]
    [SerializeField] private float distance = 5.0f; // プレイヤーからの距離
    [SerializeField] private float height = 2.0f; // プレイヤーからの高さ
    [SerializeField] private float rotationSpeed = 2.0f; // カメラ回転の感度
    [SerializeField] private float smoothSpeed = 10.0f; // カメラ追従の滑らかさ

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        // マウスカーソルをロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ターゲットが設定されているか確認
        if (target == null)
        {
            Debug.LogError("ThirdPersonCameraのTargetが設定されていません。");
            enabled = false;
        }
    }

    void Update()
    {
        // 1. 入力の取得（カメラ回転）
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Y軸の回転を制限（真上や真下を向きすぎないように）
        currentY = Mathf.Clamp(currentY, -30f, 60f); // 例として-30度から60度に制限
    }

    void LateUpdate()
    {
        // 2. 回転の適用
        // Quaternion.EulerでX, Yの回転を作成
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 3. 望ましいカメラの位置を計算
        // ターゲット位置 + 高さオフセット - (回転 * 後ろ向きベクトル * 距離)
        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 desiredPosition = targetPosition - (rotation * Vector3.forward * distance);

        // 4. カメラを目標位置へ滑らかに移動
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 5. カメラを常にターゲットへ向ける
        transform.LookAt(targetPosition);
    }
}
