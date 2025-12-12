using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public  Transform rightHandAttachPoint;
    private GameObject currentWeaponInstance;
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

        if (!rightHandAttachPoint)
        {
            Debug.LogError("設定されていない。");
            return;
        }

        GameObject newWeapon = Instantiate(data.weaponPrefab, rightHandAttachPoint);

        newWeapon.transform.localPosition = data.attatchLocalPos;
        newWeapon.transform.localRotation = Quaternion.Euler(data.attachLocalRotationEules);
        
        WeaponCollision collisionScriot = newWeapon.GetComponent<WeaponCollision>();
        if (!collisionScriot) { collisionScriot = newWeapon.GetComponent<WeaponCollision>(); }
        if (attackContorol) { attackContorol.SetcurrentWeaponCollision(collisionScriot); }

        //装備インスタンスの更新
        currentWeaponInstance = newWeapon;
    }

}
