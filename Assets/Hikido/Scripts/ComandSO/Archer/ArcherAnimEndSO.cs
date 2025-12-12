using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherAnimEndSO", menuName = "ArcherAnimEndSO "),]
public class ArcherAnimEndSO : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {
        if (!animator) return;
        string paramName = targetParm.ToString();
        animator.SetBool(paramName, false);
    }
}
