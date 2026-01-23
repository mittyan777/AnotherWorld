using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidAnimSO", menuName = "AvoidAnimSO ")]
public class AvoidAnimSO : AnimationBaseSO
{
    public override void Execute(Animator animator)
    {
        if (!animator) { return; }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        int directionID = 0;

        if (Mathf.Abs(h) > Mathf.Abs(v)) 
        {
            directionID = (h > 0) ? 1 : 3; // 右 : 左
        }
        else // 前後入力が強い
        {
            directionID = (v > 0) ? 0 : 2; // 前 : 後
        }

        animator.SetInteger("AvoidDirection", directionID);
        animator.SetTrigger("AvoidTrigger");

        Debug.Log($"回避実行: 方向ID {directionID} をセット");
    }
}
