using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public Action Attackevents;         //攻撃イベント
    public Action WalkEvents;           //移動イベント
    public Action AvoidanceEvents;      //回避イベント
    public Action AttackNormal;
    public Action AttackMagicianSkills;
    public Action AttackArcherSkills;
    
    //攻撃のイベント
    public bool AttackNormalflg 
    {
        get { return attackNormalFlg; }
        set 
        {
            if (value) 
            {
                UnityEngine.Debug.Log("イベント発火");
                AttackNormal?.Invoke();  //登録したイベントを発火
                //attackNormalFlg = false;
            }
        }
    }

   //回避のイベント
   public bool Avoidflg 
    {
        get { return walkanimFlg; }
        set
        {
            //TODO:イベント発火用アクション
            UnityEngine.Debug.Log("イベント発火");

        }
    }

}
