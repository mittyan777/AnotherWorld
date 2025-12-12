using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager_main : MonoBehaviour
{
    private EnemySpawner_main mySpawner;

    public void SetSpawner(EnemySpawner_main spawner) 
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
