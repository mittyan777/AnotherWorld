using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    //右手に装備
    public  Transform rightHandAttachPoint;

    //左手に装備
    public Transform leftHandAttackPoint;

    private GameObject currentWeaponInstance;
    private WeaponData currentWeapData;

    private AttackContorol attackContorol;

    private void Start()
    {
       attackContorol = GetComponent<AttackContorol>();
       if(attackContorol) 
       {
            attackContorol = gameObject.AddComponent<AttackContorol>();
       }
        else { Debug.LogError("あたっくこんとろーるがないよ。"); }
    }

    public void EquipWeapon(WeaponData data)
    {
        if (currentWeaponInstance)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
        }

        //現在装備データを更新
        currentWeapData = data;

        Transform targetAttachPoint;
        Vector3 localPos;
        Vector3 localRot;

        if(data.type == WeaponType.Bow) 
        {
            targetAttachPoint = leftHandAttackPoint;
            localPos = data.leftAttachLocalPos;
            localRot = data.leftAttachLocalRotationEules;

            if (!leftHandAttackPoint) 
            {
                Debug.LogError("左手のアタッチポイントが設定されていない");
                return;
            }
        }
        else 
        {
            targetAttachPoint = rightHandAttachPoint;
            localPos = data.attatchLocalPos;
            localRot = data.attachLocalRotationEules;

            if (!rightHandAttachPoint) 
            {
                Debug.LogError("右手のアタッチポイントが設定されていない");
                return;
            }
        }

        

        GameObject newWeapon = Instantiate(data.weaponPrefab, targetAttachPoint);

        newWeapon.transform.localPosition = localPos;
        newWeapon.transform.localRotation = Quaternion.Euler(localRot);
        
        WeaponCollision collisionScriot = newWeapon.GetComponentInChildren<WeaponCollision>();
        if (collisionScriot == null) { collisionScriot = newWeapon.GetComponentInChildren<WeaponCollision>(); }
        if (attackContorol != null) 
        { attackContorol.SetcurrentWeaponCollision(collisionScriot); }

        //装備インスタンスの更新
        currentWeaponInstance = newWeapon;
    }

}
