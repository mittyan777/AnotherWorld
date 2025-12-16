using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//アニメーションイベントに設定
public class AttackContorol : MonoBehaviour
{
    private WeaponCollision currentWeaponCollision;
    public void SetcurrentWeaponCollision(WeaponCollision colli) 
    {
        currentWeaponCollision = colli;
    }

    //アニメーションイベント呼び出し -> hitbox表示
    public void EnableWeaponHitBox() 
    {
        if (currentWeaponCollision) 
        { currentWeaponCollision.SetCollisionActive(true); }
    }

    //hitbox 非表示
    public void DisableWeaponhitBox() 
    {
        if (currentWeaponCollision)
        { currentWeaponCollision.SetCollisionActive(false);}
    }


}
