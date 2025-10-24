using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Achel: MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // 矢のプレハブ
    [SerializeField] private Transform shootPoint;   // 矢の発射位置（弓の先端など）
    [SerializeField] private float shootForce = 25f; // 矢の飛ぶ力

    void Update()
    {
        // 左クリック（マウスボタン0）で発射
        if (Input.GetMouseButtonDown(0))
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        // 矢を生成
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Rigidbodyがついていれば前方に力を加える
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }

        // 10秒後に自動削除（矢が増えすぎないように）
        Destroy(arrow, 10f);
    }
}

