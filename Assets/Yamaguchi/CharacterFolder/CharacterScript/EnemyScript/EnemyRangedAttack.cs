using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttackManager
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    protected override IEnumerator PerformAttack()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null)
        {
            yield break;
        }

        //�v���C���[�̕���������
        Vector3 direction = (playerTransform.position - firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //�G�{�̂̉�]�iY���j�Ɣ��˓_�̉�]�i�O���j����v������
        transform.rotation = targetRotation;
        firePoint.rotation = targetRotation;

        //�I�u�W�F�N�g�𐶐����A����
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // �������U���͐��������炷���ɏI��
        yield return null;
    }
}
