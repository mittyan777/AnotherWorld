using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttackManager
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float throwWaitingTime;

    protected override IEnumerator PerformAttackLogic()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null)
        {
            yield break;
        }

        // �U���A�j���[�V�����̑O�ɏ����҂� (���˂̃^�C�~���O����)
        yield return new WaitForSeconds(throwWaitingTime);

        // �I�u�W�F�N�g�𐶐����A����
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // �������U���͐��������炷���Ɏ��̏����ֈڂ�
        yield return null;
    }

    //�������U���̃A�j���[�V������Ԃ�ݒ�
    protected override void SetAttackAnimation()
    {
        if (enemyMovement != null)
        {
            enemyMovement.currentState = EnemyMove.EnemyState.RangedAttack;
        }
    }

    //�A�j���[�V������Ԃ����Z�b�g
    protected override void ResetAttackAnimation()
    {
        //�U���I�����ɃA�j���[�V������Ԃ����Z�b�g
        //enemyMovement.currentState = EnemyMove.EnemyState.Walk;
    }
}