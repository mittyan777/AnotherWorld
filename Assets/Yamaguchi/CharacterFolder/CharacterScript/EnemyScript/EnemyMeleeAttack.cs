using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttackManager
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float hitboxDuration = 0.3f; // �U������̎�������

    protected override void OnInit()
    {
        // �U���I�u�W�F�N�g�͏�����ԂŖ����ɂ��Ă���
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    // �U������̔����ƏI���݂̂𐧌�
    protected override IEnumerator PerformAttackLogic()
    {
        // �U�������L���ɂ���
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        // �U���������莞�Ԉێ�
        yield return new WaitForSeconds(hitboxDuration);

        // �U������𖳌��ɂ���
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }
}