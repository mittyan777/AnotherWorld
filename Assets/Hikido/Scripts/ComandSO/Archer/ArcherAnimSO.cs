using System.Collections;
using System.Collections.Generic;
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
        Debug.Log($"コマンド実行:{paramName} animationをセット");
    }
}
