using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicNormalAnimSO", menuName = "MagicNormalAnimSO ")]
public class NormalAttackSO : AnimationBaseSO
{
    private const string PARAM_ATTACK_NORMAL_TRIGGER = "AttackNormalMagic";

    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        animator.SetTrigger(PARAM_ATTACK_NORMAL_TRIGGER);
        //チェック
        UnityEngine.Debug.Log($"コマンド実行:{PARAM_ATTACK_NORMAL_TRIGGER} animationをセット");
    }
}
