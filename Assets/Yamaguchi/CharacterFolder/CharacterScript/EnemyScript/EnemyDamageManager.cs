using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    //“G‚Ìƒ_ƒ[ƒW—Ê
    [SerializeField] private int _enemyAttackDamage01;
    [SerializeField] private int _enemyAttackDamage02;
    [SerializeField] private int _enemyAttackDamage03;
    [SerializeField] private int _enemyAttackDamage04;
    [SerializeField] private int _enemyAttackDamage05;

    public int AttackDamage01 => _enemyAttackDamage01;
    public int AttackDamage02 => _enemyAttackDamage02;
    public int AttackDamage03 => _enemyAttackDamage03;
    public int AttackDamage04 => _enemyAttackDamage04;
    public int AttackDamage05 => _enemyAttackDamage05;
}
