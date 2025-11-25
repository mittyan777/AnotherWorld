using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationResetSO", menuName = "AnimationResetSO")]
public class AnimationResetSO : AnimationBaseSO
{   
    public override void Execute(Animator animator)
    {
        if (!animator) return;
        string paramName = targetParm.ToString();
        animator.SetBool(paramName, false);
    }
}
