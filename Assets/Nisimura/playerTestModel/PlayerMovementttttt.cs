using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementttttt : MonoBehaviour
{
    // 移動速度をインスペクターで設定できるようにするための変数
    public float moveSpeed = 5.0f;

    // ゲームの各フレームで実行される関数
    void Update()
    {
        // ----------------------------------------------------
        // 1. 入力の取得
        // ----------------------------------------------------

        // WASDや矢印キーからの水平方向（左右）の入力を取得
        // A, 左矢印: -1.0, D, 右矢印: 1.0, どちらも押さない: 0.0
        float horizontalInput = Input.GetAxis("Horizontal");

        // WASDや矢印キーからの垂直方向（前後）の入力を取得
        // S, 下矢印: -1.0, W, 上矢印: 1.0, どちらも押さない: 0.0
        float verticalInput = Input.GetAxis("Vertical");

        // ----------------------------------------------------
        // 2. 移動方向の計算
        // ----------------------------------------------------

        // 入力値に基づいて移動方向のベクトルを作成
        // Unityの3D空間では、Xが左右、Zが前後を指すのが一般的
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);

        // 入力がない場合に移動しないように、ベクトルを正規化（最大長さを1にする）
        // 斜め移動の速度が単一方向移動の速度よりも速くならないようにする
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // ----------------------------------------------------
        // 3. プレイヤーの移動
        // ----------------------------------------------------

        // 現在の位置、移動方向、速度、フレーム時間（Time.deltaTime）を使って移動量を計算
        // Time.deltaTimeをかけることで、フレームレートに依存しない滑らかな移動を実現
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
