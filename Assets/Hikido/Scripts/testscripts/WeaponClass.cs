using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponClass : MonoBehaviour
{
    public static WeaponClass Instance { get; private set; }

    [Header("全武器データの参照")]
    [Tooltip("プロジェクト内の全ての WeaponData ScriptableObject をここに登録")]
    [SerializeField] private WeaponData[] allWeapons;

    private Dictionary<string, WeaponData> weaponDictionary;
    private EquipmentManager equipmentManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 高速検索のために辞書を作成
            weaponDictionary = allWeapons.ToDictionary(data => data.itemKeyName);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameManager gm = GetComponent<GameManager>();
        if (gm == null || gm.Player.Length == 0 || gm.Player[0] == null)
        {
            Debug.LogError("Playerオブジェクトが見つからないため、装備システムを初期化できません。");
            return;
        }

        GameObject playerObject = gm.Player[0];
        equipmentManager = playerObject.GetComponent<EquipmentManager>();

        if (equipmentManager == null)
        {
            // EquipmentManager がなければ自動で追加
            equipmentManager = playerObject.AddComponent<EquipmentManager>();
            Debug.Log("Playerオブジェクトに EquipmentManager を自動追加しました。");
        }
    }

    //装備実行
    public bool EquipWeaponByNameKey(string weaponNameKey)
    {
        if (equipmentManager == null)
        {
            Debug.LogError("EquipmentManager がまだ初期化されていません。");
            return false;
        }

        if (weaponDictionary.TryGetValue(weaponNameKey, out WeaponData weaponToEquip))
        {
            equipmentManager.EquipWeapon(weaponToEquip);
            return true;
        }
        else
        {
            Debug.LogError($"WeaponDataが見つかりません: {weaponNameKey}");
            return false;
        }
    }
}
