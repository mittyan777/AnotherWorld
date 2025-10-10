using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{ [Header("回転の中心となるオブジェクト")]
    public Transform target;

    [Header("回転速度 (度/秒)")]
    public float rotationSpeed = 20f;

    [Header("カメラと中心の距離")]
    public float distance = 10f;

    [Header("カメラの高さ")]
    public float height = 3f;

    private float angle = 0f;

    void Update()
    {
        if (target == null) return;

        // 回転角度を更新
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        // ラジアンに変換
        float rad = angle * Mathf.Deg2Rad;

        // カメラの新しい位置を計算
        float x = target.position.x + Mathf.Cos(rad) * distance;
        float z = target.position.z + Mathf.Sin(rad) * distance;
        float y = target.position.y + height;

        // カメラの位置を更新
        transform.position = new Vector3(x, y, z);

        // 常にターゲットを注視
        transform.LookAt(target);
    }
}
