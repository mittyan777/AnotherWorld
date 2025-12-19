using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

public class WeaponCollision : MonoBehaviour
{
    public Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        if (!weaponCollider)
        {
            UnityEngine.Debug.LogWarning("Weapnコライダーなし");
        }

        //待機中は常に無効
        SetCollisionActive(false);
    }
    

    public void SetCollisionActive(bool isActive) 
    {
        if (weaponCollider != null) 
        {
            weaponCollider.enabled = isActive;
        }
    }

    //ダメージ計算はゲームマネージャからエネミーが数値取得　ー＞　接触判定チェックのみ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) { UnityEngine.Debug.Log("当たっているよ"); }
    }

}
