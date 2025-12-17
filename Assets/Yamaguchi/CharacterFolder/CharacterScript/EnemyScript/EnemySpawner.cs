using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //スポーンエリアの2点間
    private Transform pointA;
    private Transform pointB;
    
    //生成対象のEnemyオブジェクト
    [SerializeField] private GameObject[] enemyPrefabs;
    //生成失敗を何回まで許容するか。
    [SerializeField] private int maxAttempts = 10;
    //スポーン間隔
    [SerializeField] private float spawnInterval;
    //スポーンする敵の最大数
    [SerializeField] private int maxSpawn;
    //現在のスポーン数
    public int currentSpawnCount { get; private set; }
    private float timer;

    //スポナーにセットされている敵の数引き渡し用変数
    public int spawnEnemyCount { get; private set; }
    void Start()
    {
        //子オブジェクトの2点を取得
        if (transform.childCount >= 2)
        {
            pointA = transform.GetChild(0);
            pointB = transform.GetChild(1);
        }
        else
        {
            Debug.LogError("子オブジェクトないよ");
        }

        currentSpawnCount = 0;
        spawnEnemyCount = enemyPrefabs.Length;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval&&currentSpawnCount < maxSpawn)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    // スポーン数を減らすための公開メソッドを定義
    public void DecrementSpawnCount()
    {
        // スポーン数を減らす
        // 0未満にならないようガードする
        currentSpawnCount = Mathf.Max(0, currentSpawnCount - 1);
    }

    private void SpawnEnemy()
    {
        currentSpawnCount++;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("敵のプレハブ入れて");
            return;
        }

        //敵の種類をランダムで選択
        GameObject enemyToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];

        //NavMesh上の有効なランダム位置を検索
        Vector3 spawnPosition = GetRandomNavMeshPosition();

        if (spawnPosition != Vector3.zero)
        {
            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            newEnemy.transform.rotation = Quaternion.Euler(0f, newEnemy.transform.rotation.eulerAngles.y, 0f);
            //生成された敵オブジェクトにスポナーの参照を渡す
            EnemySpawnerManager enemySpawnerManager = newEnemy.GetComponent<EnemySpawnerManager>();
            if (enemySpawnerManager != null)
            {
                enemySpawnerManager.SetSpawner(this);
            }
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        //最小座標と最大座標を決定
        //ポイントA,Bの位置を比べて、変数に格納
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        float minZ = Mathf.Min(pointA.position.z, pointB.position.z);
        float maxZ = Mathf.Max(pointA.position.z, pointB.position.z);

        //ランダムな生成を確実に成功させる為の繰り返し。
        for (int i = 0; i < maxAttempts; i++)
        {
            //スポーン範囲内でランダムな点を生成
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomZ = UnityEngine.Random.Range(minZ, maxZ);

            //スポーンさせるポジションYを決める
            float randomY = (pointA.position.y + pointB.position.y) / 2f;
            Vector3 randomPoint = new Vector3(randomX, randomY, randomZ);

            NavMeshHit hit;

            //NavMesh.SamplePosition：指定範囲内にEnemyが動ける床があるかを探す機能
            //randomPointを渡すことでどの点を基準にして探索するかを指定してあげる
            //out hitにはNavMeshHit型の変数が入る。役割は動ける床が見つかったらその床の座標を返す
            //1.0fはrandomPointを基準に何メートル先まで探査するか
            //NavMesh.AllAreasはすべての歩行可能なNavMeshを対象にする
            if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
            {
                //Debug.Log("スポーンできる");
                //NavMesh上の有効な位置が見つかった
                return hit.position;                
            }
        }

        //試行回数を超えても見つからなかった場合
        return Vector3.zero;
    }
}
