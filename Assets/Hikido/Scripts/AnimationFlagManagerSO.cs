using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationSO", menuName = "AnimationSO")]

public class AnimationFlagManagerSO : ScriptableObject
{
    [Header("管理用アニメーションフラグ")]
    [SerializeField] private bool attackNormalFlg = false; 
    [SerializeField] private bool attackSkilFlag = false;
    [SerializeField] private bool walkanimFlg = false;
    [SerializeField] private bool avoidanceFlg = false;

    [Header("各通知イベント")]
    public Action Attackevents;
    public Action WalkEvents;
    public Action AvoidanceEvents;
    public Action AttackNormal;
    public Action AttackMagicianSkills;
    public Action AttackArcherSkills;
    
    //攻撃のイベント
    public bool AttackNormalflg 
    {
        get { return attackNormalFlg; }
        set 
        {
            if(attackNormalFlg == value || value == false) { return; }
            attackNormalFlg = value;
            AttackNormal?.Invoke();  //登録したイベントを発火
        }
    }

}
