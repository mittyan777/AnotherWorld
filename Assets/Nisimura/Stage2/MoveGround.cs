using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [SerializeField] private Transform pointA; // 地点A
    [SerializeField] private Transform pointB; // 地点B
    [SerializeField] private float speed = 1.0f; // 往復にかかる速度

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // 0.0 ～ 1.0 の間を往復する値を作る
        float time = Mathf.PingPong(Time.time * speed, 1.0f);

        // AとBの間をtime（0～1）の割合で移動する
        transform.position = Vector3.Lerp(pointA.position, pointB.position, time);
    }
}
