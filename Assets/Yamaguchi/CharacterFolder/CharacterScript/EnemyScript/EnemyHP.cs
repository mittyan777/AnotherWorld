using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private int enemyMaxHP;
    private int enemyCurrentHP;
    EnemyDropManager dropManager;

    private void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        dropManager = GetComponent<EnemyDropManager>();
    }

    private void Update()
    {

        //テスト用ダメージ
        if (Input.GetKeyDown(KeyCode.R))
        {
            enemyCurrentHP -= 5;
            Debug.Log("Enemyに5のダメージ");
        }
        if (enemyCurrentHP <= 0)
        { 
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        dropManager.DropItem();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ThrowingObject throwing = other.GetComponent<ThrowingObject>();
        if (other.CompareTag("Finish"))
        {
            Debug.Log(throwing.stonePower + "ダメージ");
            enemyCurrentHP -= throwing.stonePower;
        }
    }
}
