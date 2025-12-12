using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MajicianAnimSO", menuName = "MajicianAnimSO ")]
public class Majician_SkilAnimSO : AnimationBaseSO
{
    private const string PARAM_ATTACK_MAJIC1_TRIGGER = "MajickSkil_1";
    private const string PARAM_ATTACK_MAJIC2_TRIGGER = "MajickSkil_2";
    private const string PARAM_ATTACK_MAJIC3_TRIGGER = "MajickSkil_3";

    public int ActiveSkillIndex { get; set; } = 0;

    public override void Execute(Animator animator)
    {
        if (!animator) { return; }
        switch (ActiveSkillIndex)
        {
            case 1:
                animator.SetTrigger(PARAM_ATTACK_MAJIC1_TRIGGER);
                break;
            case 2:
                animator.SetTrigger(PARAM_ATTACK_MAJIC2_TRIGGER);
                break;
            case 3:
                animator.SetTrigger(PARAM_ATTACK_MAJIC3_TRIGGER);
                break;
            default:
                UnityEngine.Debug.LogWarning("ActiveSkillIndexが設定されていません。");
                break;
        }
        Debug.Log("マジシャンのアニメーションが発動している");

        ActiveSkillIndex = 0;

    }
}
