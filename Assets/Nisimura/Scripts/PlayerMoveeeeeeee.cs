using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveeeeeeee : MonoBehaviour
{
    // インスペクターで設定する項目
    [Header("移動設定")]
    [Tooltip("プレイヤーの移動速度")]
    public float moveSpeed = 5.0f;
    [Tooltip("回転速度（カメラの方向に向く速さ）")]
    public float rotationSpeed = 10.0f;

    // カメラのTransform (TPSカメラ、またはメインカメラ)
    [Header("カメラ連携")]
    [Tooltip("カメラオブジェクトのTransformをアタッチ")]
    public Transform cameraTransform;

    private CharacterController controller; // プレイヤーの移動に使用するコンポーネント
    private Vector3 moveDirection;          // 計算された移動方向

    void Start()
    {
        // CharacterControllerコンポーネントを取得
        controller = GetComponent<CharacterController>();

        // CharacterControllerがない場合は警告を出してスクリプトを無効化
        if (controller == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController component on the same GameObject.");
            enabled = false;
        }

        // cameraTransformが未設定の場合は、メインカメラを探して設定
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("Main Camera not found. Please set the Camera Transform manually.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        // 1. 入力の取得
        float horizontal = Input.GetAxis("Horizontal"); // A/Dまたは左スティックX
        float vertical = Input.GetAxis("Vertical");   // W/Sまたは左スティックY

        // 2. 移動方向の計算
        // 入力ベクトルを作成
        Vector3 inputVector = new Vector3(horizontal, 0f, vertical).normalized;

        // 入力がなければ処理を終了
        if (inputVector.magnitude < 0.1f)
        {
            // 移動を停止し、重力処理のみ行う
            moveDirection.x = 0;
            moveDirection.z = 0;

            // 重力適用（CharacterControllerは自動で重力をかけないため、ここで適用が必要）
            ApplyGravity();

            // 最後にコントローラーで移動
            controller.Move(moveDirection * Time.deltaTime);
            return;
        }

        // 3. カメラの向きに基づいた移動方向の変換
        // カメラのY軸回転のみを取得（上下の傾きは無視）
        Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // 入力方向をカメラの回転に合わせてワールド座標系に変換
        Vector3 desiredMoveDirection = cameraRotation * inputVector;

        // 4. 回転処理 (プレイヤーを進行方向に向ける)
        Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);

        // 滑らかに回転
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 5. 最終的な移動方向を設定
        // Y軸の速度（重力など）を保持したまま、水平方向の速度を更新
        float ySpeed = moveDirection.y;
        moveDirection = desiredMoveDirection * moveSpeed;
        moveDirection.y = ySpeed; // Y軸速度を戻す

        // 6. 重力適用
        ApplyGravity();

        // 7. 実際の移動実行
        controller.Move(moveDirection * Time.deltaTime);
    }

    // 重力処理をカプセル化
    void ApplyGravity()
    {
        // プレイヤーが地面に接地していない場合、重力を適用
        if (!controller.isGrounded)
        {
            // 簡単な重力加速度の適用 (例: -9.8f)
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // 地面に接地している場合、Y軸の速度をリセット（わずかに地面に押し付ける値にしておく）
            if (moveDirection.y < 0)
            {
                moveDirection.y = -2f;
            }
        }
    }
}
