using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventry : MonoBehaviour
{
    public static PlayerInventry Instance { get; private set; }

    [Header("データ保持用")]
    public List<string> purchasedWeaponKeys = new List<string>();
    public string eqippedWeaponkey = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 親がいない場合のみDontDestroy。GameManagerの子ならこれ自体は不要
            if (transform.parent == null) { DontDestroyOnLoad(gameObject); }
        }
        else { Destroy(gameObject); }
    }

    public void AddWeaponToInventory(string weaponKey)
    {
        if (!purchasedWeaponKeys.Contains(weaponKey))
        {
            purchasedWeaponKeys.Add(weaponKey);
            // 最初の一本なら自動装備
            if (string.IsNullOrEmpty(eqippedWeaponkey)) { SetEquippedWeapon(weaponKey); }
        }
    }

    public void SetEquippedWeapon(string weaponKey)
    {
        if (!purchasedWeaponKeys.Contains(weaponKey)) return;

        eqippedWeaponkey = weaponKey;

        // 現在のシーンのプレイヤーに装備させる
        if (WeaponClass_main.Instance != null)
        {
            WeaponClass_main.Instance.EquipWeaponByNameKey(weaponKey);
        }
    }
}