using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private int attackDamage01;
    private int attackDamage02;
    private int attackDamage03;

    [SerializeField] private GameObject bossManagerObj;
    private BossManager bossManager;

    private void Start()
    {
        bossManager=bossManagerObj.GetComponent<BossManager>();
        attackDamage01 = bossManager.AttackDamage01;
        attackDamage02 = bossManager.AttackDamage02;
        attackDamage03 = bossManager.AttackDamage03;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bossManager.bossBattle)
        {
            return;
        }
        if (other.gameObject.CompareTag("AttackArea01"))
        {
            Debug.Log(attackDamage01);
        }
        if (other.gameObject.CompareTag("AttackArea02"))
        {
            Debug.Log(attackDamage02);
        }
        if (other.gameObject.CompareTag("AttackArea03"))
        {
            Debug.Log(attackDamage03);
        }
    }
}
