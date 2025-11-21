using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationResetSO", menuName = "AnimationResetSO")]
public class AnimationResetSO : AnimationBaseSO
{   
    [SerializeField] private AnimationFlagManagerSO _animflgSO;

    public override void Execute(Animator animator)
    {
        if (!animator || !_animflgSO) return;
        string paramName = targetParm.ToString();
        animator.SetBool(paramName, false);
    }
}
