using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherAnimSO", menuName = "ArcherAnimSO ")]
public class ArcherAnimSO : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        string paramName = targetParm.ToString();
        animator.SetBool(paramName, true);
        //チェック
        UnityEngine.Debug.Log($"コマンド実行:{paramName} animationをセット");
    }
}
