using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
     [Header("回転の中心となるターゲット")]
    [SerializeField] private Transform target;

    [Header("カメラの回転速度 (度/秒)")]
    [SerializeField] private float rotationSpeed = 30f;

    [Header("回転半径")]
    [SerializeField] private float distance = 5f;

    [Header("回転角度の範囲 (例: 360で一周)")]
    [SerializeField] private float maxAngle = 360f;

    [Header("回転軸")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private float currentAngle = 0f;

    void Update()
    {
        if (target == null) return;

        // 時間経過で角度を増やす
        currentAngle += rotationSpeed * Time.deltaTime;

        // 最大角度を超えたら止める or ループ
        if (currentAngle > maxAngle)
        {
            currentAngle = 0f; // ←ループしたくない場合は return; に変更
        }

        // 回転位置を計算
        Quaternion rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
        Vector3 offset = rotation * (Vector3.back * distance);

        // カメラ位置更新
        transform.position = target.position + offset;

        // カメラをターゲットの方向に向ける
        transform.LookAt(target);
    }
}
