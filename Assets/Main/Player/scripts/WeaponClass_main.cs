using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponClass_main : MonoBehaviour
{
    public static WeaponClass_main Instance { get; private set; }

    [SerializeField] private WeaponData[] allWeapons;
    private Dictionary<string, WeaponData> weaponDictionary;
    private EquipmentManager currentEquipmentManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent == null) { DontDestroyOnLoad(gameObject); }
            weaponDictionary = allWeapons.ToDictionary(data => data.itemKeyName);
        }
        else { Destroy(gameObject); }
    }

    //今のシーンのプレイヤーからEquipmentManagerを見つけ出す
    private bool RefreshPlayerReference()
    {
        GameManager gm = GetComponent<GameManager>();
        GameObject playerObj = null;

        //GameManagerのPlayer[0]が今のシーンのものを指しているか確認
        if (gm != null && gm.Player != null && gm.Player.Length > 0 && gm.Player[0] != null)
        {
            playerObj = gm.Player[0];
        }
        else
        {
            //GameManagerで見つからない場合の予備（タグ検索）
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerObj == null) return false;

        //参照が古いor未取得なら取得
        if (currentEquipmentManager == null || currentEquipmentManager.gameObject != playerObj)
        {
            currentEquipmentManager = playerObj.GetComponent<EquipmentManager>();
        }

        return currentEquipmentManager != null;
    }

    public bool EquipWeaponByNameKey(string weaponNameKey)
    {
        if (!RefreshPlayerReference()) return false;

        if (weaponDictionary.TryGetValue(weaponNameKey, out WeaponData data))
        {
            currentEquipmentManager.EquipWeapon(data);
            return true;
        }
        return false;
    }
}