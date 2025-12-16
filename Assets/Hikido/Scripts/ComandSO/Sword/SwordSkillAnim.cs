using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordSkillAnimSO", menuName = "SwordSkillAnimSO ")]
public class SwordSkillAnime : AnimationBaseSO
{
    private const string PARAM_COMBO_TRIGGER = "ComboTrigger";
    //private const string PARAM_COMBO_COUNT = "ComboCount";

    //継承しているので残しているが使わない
    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        Debug.Log("警告: Execute()ではなくExecuteCombo()を使用してください。");
    }

    public void ExecuteCombo(Animator animator,int comboCount) 
   {
        if (!animator) { return; }
        //animator.SetInteger(PARAM_COMBO_COUNT, comboCount);
        animator.SetTrigger(PARAM_COMBO_TRIGGER);
        Debug.Log($"コマンド実行: Combo{comboCount}を設定し、{PARAM_COMBO_TRIGGER}を発火");
   }
}
