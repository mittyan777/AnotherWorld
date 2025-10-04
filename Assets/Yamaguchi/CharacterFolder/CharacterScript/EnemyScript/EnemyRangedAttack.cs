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

        //プレイヤーの方向を向く
        Vector3 direction = (playerTransform.position - firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //敵本体の回転（Y軸）と発射点の回転（前方）を一致させる
        transform.rotation = targetRotation;
        firePoint.rotation = targetRotation;

        //オブジェクトを生成し、発射
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 遠距離攻撃は生成したらすぐに終了
        yield return null;
    }
}
