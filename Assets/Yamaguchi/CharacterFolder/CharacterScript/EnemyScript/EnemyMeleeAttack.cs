using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttackManager
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float hitboxDuration = 0.5f;

    protected override void OnInit()
    {
        // �U���I�u�W�F�N�g�͏�����ԂŖ����ɂ��Ă���
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    protected override IEnumerator PerformAttack()
    {
        //�U���I�u�W�F�N�g��L���ɂ��� (�U�����蔭��)
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        //�U���������莞�Ԉێ� (0.5�b)
        yield return new WaitForSeconds(hitboxDuration);

        //�U���I�u�W�F�N�g�𖳌��ɂ��� (�U������I��)
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }
}
