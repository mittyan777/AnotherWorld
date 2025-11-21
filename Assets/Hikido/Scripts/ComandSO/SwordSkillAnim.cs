using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordSkillAnimSO", menuName = "SwordSkillAnimSO ")]
public class SwordSkillAnime : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {
        if(!animator) { return; }
        string paramName = targetParm.ToString();
        animator.SetTrigger(paramName);
        //チェック
        Debug.Log($"コマンド実行:{paramName} animationをセット");
    }
}
