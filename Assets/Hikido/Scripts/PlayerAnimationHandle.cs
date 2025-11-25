using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandle : MonoBehaviour
{
    [SerializeField] private PlayerAnimation animationRouter;

    public void AttackAnimation_NormalEnd()
    {
        if (animationRouter != null)
        {
            animationRouter.AttackAnimation_NormalEnd();
        }
        else
        {
            Debug.LogError("AnimationRouter (PlayerAnimation) ‚Ö‚ÌQÆ‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñB", this);
        }
    }
}
