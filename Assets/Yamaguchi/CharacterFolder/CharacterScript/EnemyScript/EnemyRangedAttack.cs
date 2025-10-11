using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttackManager
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    protected override IEnumerator PerformAttackLogic()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null)
        {
            yield break;
        }

        // 攻撃アニメーションの前に少し待つ (発射のタイミング調整)
        yield return new WaitForSeconds(0.1f);

        // オブジェクトを生成し、発射
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 遠距離攻撃は生成したらすぐに次の処理へ移る
        yield return null;
    }
}