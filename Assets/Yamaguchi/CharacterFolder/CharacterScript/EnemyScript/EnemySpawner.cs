using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //�X�|�[���G���A��2�_��
    private Transform pointA;
    private Transform pointB;
    
    //�����Ώۂ�Enemy�I�u�W�F�N�g
    [SerializeField] private GameObject[] enemyPrefabs;

    //�������s������܂ŋ��e���邩�B
    [SerializeField] private int maxAttempts = 10;
    //�X�|�[���Ԋu
    [SerializeField] private float spawnInterval;

    private float timer;

    void Start()
    {
        //�q�I�u�W�F�N�g��2�_���擾
        if (transform.childCount >= 2)
        {
            pointA = transform.GetChild(0);
            pointB = transform.GetChild(1);
        }
        else
        {
            Debug.LogError("�q�I�u�W�F�N�g�Ȃ���");
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("�G�̃v���n�u�����");
            return;
        }

        //�G�̎�ނ������_���őI��
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        //NavMesh��̗L���ȃ����_���ʒu������
        Vector3 spawnPosition = GetRandomNavMeshPosition();

        if (spawnPosition != Vector3.zero)
        {
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        //�ŏ����W�ƍő���W������
        //�|�C���gA,B�̈ʒu���ׂāA�ϐ��Ɋi�[
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        float minZ = Mathf.Min(pointA.position.z, pointB.position.z);
        float maxZ = Mathf.Max(pointA.position.z, pointB.position.z);

        //�����_���Ȑ������m���ɐ���������ׂ̌J��Ԃ��B
        for (int i = 0; i < maxAttempts; i++)
        {
            //�X�|�[���͈͓��Ń����_���ȓ_�𐶐�
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            //�X�|�[��������|�W�V����Y�����߂�
            float randomY = (pointA.position.y + pointB.position.y) / 2f;
            Vector3 randomPoint = new Vector3(randomX, randomY, randomZ);

            NavMeshHit hit;

            //NavMesh.SamplePosition�F�w��͈͓���Enemy�������鏰�����邩��T���@�\
            //randomPoint��n�����Ƃłǂ̓_����ɂ��ĒT�����邩���w�肵�Ă�����
            //out hit�ɂ�NavMeshHit�^�̕ϐ�������B�����͓����鏰�����������炻�̏��̍��W��Ԃ�
            //1.0f��randomPoint����ɉ����[�g����܂ŒT�����邩
            //NavMesh.AllAreas�͂��ׂĂ̕��s�\��NavMesh��Ώۂɂ���
            if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
            {
                Debug.Log("�X�|�[���ł���");
                //NavMesh��̗L���Ȉʒu����������
                return hit.position;                
            }
        }

        //���s�񐔂𒴂��Ă�������Ȃ������ꍇ
        return Vector3.zero;
    }
}
