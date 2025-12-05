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
    [SerializeField] private bool avoidanceFlg = false;    //回避フラグ
    [SerializeField] private bool attackArcherFlg = false; //アーチャーフラグ
    [SerializeField] private bool archerRecoilFlg = false; //アーチャー発射フラグ

    [Header("各通知イベント")]
    public Action Attackevents;          //攻撃イベント
    public Action WalkEvents;            //移動イベント
    public Action AvoidanceEvents;       //回避イベント
    public Action AttackNormal;
    public Action AttackMagicianSkills;  //マジシャンスキルイベント
    public Action AttackArcherSkills;    //アーチャースキルイベント
    public Action ArcherRecoil;          //アーチャー発射イベント
                                       
    //攻撃のイベント
    public bool AttackNormalflg 
    {
        get { return attackNormalFlg; }
        set 
        {
            if(attackNormalFlg == value) { return; }

            attackNormalFlg = value;

            if (value) 
            {
                UnityEngine.Debug.Log("イベント発火");
                AttackNormal?.Invoke();  //登録したイベントを発火
            }
        }
    }

   //回避のイベント
   public bool Avoidflg
    {
        get { return avoidanceFlg; }
        set
        {
            if (avoidanceFlg == value){ return; }
            avoidanceFlg = value;

            if (value)
            {
                AvoidanceEvents?.Invoke();
                UnityEngine.Debug.Log("回避イベント発火");
            }
        }
    }

    //アーチャーのイベント
    public bool ArcherSkilflg 
    {
        get { return attackArcherFlg; }
        set
        {
            if (attackArcherFlg == value) { return; }
            attackArcherFlg = value;
            if (value) 
            {
                AttackArcherSkills?.Invoke();
                UnityEngine.Debug.Log("アーチャーイベント発火");
            }
        }
    }

    //アーチャーの発射イベント
    public bool ArcherRecoileFlg 
    {
        get { return archerRecoilFlg; }
        set 
        {
            if (archerRecoilFlg == value) { return; }
            archerRecoilFlg = value;
            if (value) 
            {
                ArcherRecoil?.Invoke();
                UnityEngine.Debug.Log("エイム時のイベント発火");
            }
        }
    }

}
