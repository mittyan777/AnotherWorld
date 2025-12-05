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
            Debug.LogError("AnimationRouter (PlayerAnimation) への参照が設定されていません。", this);
        }
    }

    //回避終了アニメーション実行用の関数
    public void AvoidAnimationEnd() 
    {
        if (animationRouter != null) { animationRouter.AvoidAnimationEnd(); }
        else { Debug.LogError("AnimationRouterなし。"); }
    }

    //アーチャーアニメーションの終了用関数
    public void ArcherAnimationEnd() 
    {
        if(animationRouter != null) { animationRouter.AttackAnimation_ArcherEnd(); }
        else { Debug.LogError("animationRouterなし"); }
    }


    //アーチャーの発射アニメーション終了
    public void ArcherrecoilEnd() 
    {
        if(animationRouter != null) { animationRouter.ArcherRecoilEndAnim(); }
        else { Debug.LogError("aimatorRouterなし"); }
    }
}
