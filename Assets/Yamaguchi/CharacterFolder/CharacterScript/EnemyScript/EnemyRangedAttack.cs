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

        // 攻撃アニメーションの前に少し待つ (発射のタイミング調整)
        yield return new WaitForSeconds(throwWaitingTime);

        // オブジェクトを生成し、発射
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 遠距離攻撃は生成したらすぐに次の処理へ移る
        yield return null;
    }

    //遠距離攻撃のアニメーション状態を設定
    protected override void SetAttackAnimation()
    {
        if (enemyMovement != null)
        {
            enemyMovement.currentState = EnemyMove.EnemyState.RangedAttack;
        }
    }

    //アニメーション状態をリセット
    protected override void ResetAttackAnimation()
    {
        //攻撃終了時にアニメーション状態をリセット
        //enemyMovement.currentState = EnemyMove.EnemyState.Walk;
    }
}