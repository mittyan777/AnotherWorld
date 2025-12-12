using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherADSEndSO : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {

        animator.SetBool("IsMoving", false);
        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", 0f);

        int adsLayerIndex = animator.GetLayerIndex("ADS Layer");
        if (adsLayerIndex != -1)
        {
            animator.SetLayerWeight(adsLayerIndex, 0f);
        }

        UnityEngine.Debug.Log("移動アニメーション終了." );
    }
}
