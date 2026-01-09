using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public string rightHandName = "RightHand";
    public string leftHandName = "LeftHand";
    public Transform rightHandAttachPoint;
    public Transform leftHandAttackPoint;

    private GameObject currentWeaponInstance;
    private AttackContorol attackContorol;

    private void Start()
    {
        // 1. 攻撃制御クラスの取得
        attackContorol = GetComponent<AttackContorol>();
        if (attackContorol == null) { attackContorol = gameObject.AddComponent<AttackContorol>(); }

        // 2. ボーンの自動検索（シーン遷移対策）
        RefreshAttachPoints();

        // 3. シーン開始時に保存された武器を装備
        if (PlayerInventry.Instance != null && !string.IsNullOrEmpty(PlayerInventry.Instance.eqippedWeaponkey))
        {
            // 全体の初期化を待つため1フレーム後に実行
            Invoke(nameof(AutoEquip), 0.01f);
        }
    }

    private void AutoEquip()
    {
        WeaponClass_main.Instance.EquipWeaponByNameKey(PlayerInventry.Instance.eqippedWeaponkey);
    }

    public void RefreshAttachPoints()
    {
        if (!rightHandAttachPoint) rightHandAttachPoint = FindChildRecursive(transform, rightHandName);
        if (!leftHandAttackPoint) leftHandAttackPoint = FindChildRecursive(transform, leftHandName);
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }

    public void EquipWeapon(WeaponData data)
    {
        if (currentWeaponInstance) Destroy(currentWeaponInstance);

        Transform targetPoint = (data.type == WeaponType.Bow) ? leftHandAttackPoint : rightHandAttachPoint;
        if (!targetPoint) { RefreshAttachPoints(); targetPoint = (data.type == WeaponType.Bow) ? leftHandAttackPoint : rightHandAttachPoint; }

        if (!targetPoint) { Debug.LogError("アタッチポイントが見つかりません"); return; }

        GameObject newWeapon = Instantiate(data.weaponPrefab, targetPoint);
        newWeapon.transform.localPosition = (data.type == WeaponType.Bow) ? data.leftAttachLocalPos : data.attatchLocalPos;
        newWeapon.transform.localRotation = Quaternion.Euler((data.type == WeaponType.Bow) ? data.leftAttachLocalRotationEules : data.attachLocalRotationEules);

        WeaponCollision col = newWeapon.GetComponentInChildren<WeaponCollision>();
        if (attackContorol != null) attackContorol.SetcurrentWeaponCollision(col);

        currentWeaponInstance = newWeapon;
    }
}