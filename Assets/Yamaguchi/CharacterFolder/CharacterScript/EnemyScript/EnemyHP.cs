using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private int enemyMaxHP;
    private int enemyCurrentHP;
    EnemyDropManager dropManager;

    protected EnemyMove enemyMove;
    [SerializeField] private float damageTime;
    [SerializeField] private float breakTime;

    private void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        dropManager = GetComponent<EnemyDropManager>();

        enemyMove = GetComponentInParent<EnemyMove>();
        if (enemyMove == null)
        {
            Debug.LogError(gameObject.name + ": EnemyMove��������܂���B", this);
            this.enabled = false;
            return;
        }
    }

    private void Update()
    {

        //�e�X�g�p�_���[�W
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetDamege();
            Debug.Log("Enemy��5�̃_���[�W");
        }
    }

    private void EnemyDied()
    {
        enemyMove.ForceSetState(EnemyMove.EnemyState.death);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;  // �����Ɉړ����~
        }

        enemyMove.enabled = false;
        // ���S�������R���[�`���Ŏ��s
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        if (dropManager != null)
        {
            dropManager.DropItem();
        }
        yield return new WaitForSeconds(damageTime);
        Destroy(gameObject, breakTime);
    }

    private IEnumerator DamageSequence() 
    {
        enemyMove.SetIsStoppedByAttack(true);

        yield return new WaitForSeconds(damageTime);

        if (enemyCurrentHP > 0)
        {
            enemyMove.ForceSetState(EnemyMove.EnemyState.Idle);

            if (enemyMove.GetComponent<NavMeshAgent>() != null)
            {
                enemyMove.GetComponent<NavMeshAgent>().ResetPath();
            }

            enemyMove.SetIsStoppedByAttack(false);
        }
    }

    private void GetDamege()
    {
        if (enemyMove.currentState == EnemyMove.EnemyState.death) return;
        if (enemyCurrentHP <= 0) return;
        //TODO:�����ł�Player����󂯎�����_���[�W������B
        enemyCurrentHP -= 5;

        //���S���Ă���̂Ń_���[�W�̃A�j���[�V�����͕s�v
        if (enemyCurrentHP <= 0)
        {
            //enemyMove.currentState = EnemyMove.EnemyState.death;
            //���S��ԂɂȂ�����A���̃R���[�`����S�Ē�~
            StopAllCoroutines();
            EnemyDied();
            return;
        }

        //HP��0���ᖳ����ΐ����Ă���Ƃ������Ȃ̂Ń_���[�W�̏���������
        if (enemyMove.currentState != EnemyMove.EnemyState.damage)
        { 
            enemyMove.currentState = EnemyMove.EnemyState.damage;
            StopAllCoroutines();

            enemyMove.ForceSetState(EnemyMove.EnemyState.damage);

            StartCoroutine(DamageSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            GetDamege();
        }
        else 
        {
            return;
        }
    }
}
