using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeBossDeath : MonoBehaviour
{
    private BossManager bossManager;

    private void Start()
    {
        bossManager = FindObjectOfType<BossManager>();
        if (bossManager != null)
        {
            Debug.LogError("BossManerger‚È‚¢", this);
        }
    }


}
