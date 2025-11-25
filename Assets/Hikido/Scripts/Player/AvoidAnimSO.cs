using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidAnimSO", menuName = "AvoidAnimSO ")]
public class AvoidAnimSO : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        string paramName = targetParm.ToString();
        animator.SetTrigger(paramName);
        //チェック
        Debug.Log($"コマンド実行:{paramName} animationをセット");
    }
}
