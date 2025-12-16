using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventry : MonoBehaviour
{
    //購入した武器のリスト
    public List<string> purchasedWeaponKeys = new List<string>();

    public string eqippedWeaponkey = "";

    //シングルトン
    public static PlayerInventry Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
            if(transform.parent == null) { DontDestroyOnLoad(gameObject); }
        }
        else { Destroy(gameObject); }
    }

    //購入成功時に呼び出す関数
    public void AddWeaponToInventory(string weaponKey) 
    {
        if (!purchasedWeaponKeys.Contains(weaponKey))
        {
            purchasedWeaponKeys.Add(weaponKey);
            Debug.Log($"インベントリに武器を追加しました: {weaponKey}");
            if (eqippedWeaponkey == "")
            {
                SetEquippedWeapon(weaponKey);
            }
        }
    }

    public void SetEquippedWeapon(string weaponKey)
    {
        //購入済みのアイテムであるかチェック
        if (!purchasedWeaponKeys.Contains(weaponKey))
        {
            Debug.LogWarning($"インベントリにない武器は装備できません: {weaponKey}");
            return;
        }

        eqippedWeaponkey = weaponKey;
        Debug.Log($"装備武器キーを更新: {weaponKey}");

        WeaponClass.Instance?.EquipWeaponByNameKey(weaponKey);
    }



}
