using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationSO", menuName = "AnimationSO")]

public class AnimationFlagManagerSO : ScriptableObject
{
    [Header("管理用アニメーションフラグ")]
    public bool attackNormalFlg = false;
    public bool attackSkilFlag = false;
    public bool walkanimFlg = false;
    public bool avoidanceFlg = false;

    /// <summary> /// 攻撃時のイベント /// </summary>
    public Action Attackevents;

    /// <summary> /// 歩行時のイベント /// </summary>
    public Action WalkEvents;

    /// <summary> /// 回避時のイベント /// </summary>
    public Action AvoidanceEvents;

    /// <summary> /// 通常攻撃のイベント /// </summary>
    public Action AttackNormal;

    /// <summary> /// マジシャンスキル /// </summary>
    public Action AttackMagicianSkills;

    /// <summary> /// アーチャースキル /// </summary>
    public Action AttackArcherSkills;

}
