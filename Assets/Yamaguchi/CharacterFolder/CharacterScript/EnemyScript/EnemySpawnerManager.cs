using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private EnemySpawner mySpawner;

    public void SetSpawner(EnemySpawner spawner) 
    { 
        mySpawner = spawner;
    }

    private void OnDestroy()
    {
        if (mySpawner != null)
        {
            mySpawner.DecrementSpawnCount();
        }
    } 
}
