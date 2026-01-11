using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandle_main : MonoBehaviour
{
    [SerializeField] private PlayerAnimation_main animationRouter;
    private void Start()
    {
        animationRouter = GetComponent<PlayerAnimation_main>();
    }

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

    //アーチャーの発射アニメーション終了
    public void ArcherrecoilEnd() 
    {
        if(animationRouter != null) { animationRouter.ArcherRecoilEndAnim(); }
        else { Debug.LogError("aimatorRouterなし"); }
    }
}
