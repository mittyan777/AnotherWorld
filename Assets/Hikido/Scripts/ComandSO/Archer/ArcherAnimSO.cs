using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherAnimSO", menuName = "ArcherAnimSO ")]
public class ArcherAnimSO : AnimationBaseSO
{
    private const string PARAM_ATTACK_NORMAL_TRIGGER = "AttackNormalArcher";

    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        animator.SetTrigger(PARAM_ATTACK_NORMAL_TRIGGER);
        //チェック
        UnityEngine.Debug.Log($"コマンド実行:{PARAM_ATTACK_NORMAL_TRIGGER} animationをセット");


    }
}
