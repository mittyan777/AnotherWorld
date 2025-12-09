using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private bool bossBattle;
    [SerializeField] private GameObject activeBossObj;
    [SerializeField] private GameObject activeCanvas;
    [SerializeField] private float deleteTime;

    private void Start()
    {
        bossBattle = false;
        activeBossObj.SetActive(false);
        activeCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bossBattle)
        {
            BossBatrleStart();
            bossBattle = true;
        }
    }

    private void BossBatrleStart() 
    {
        activeBossObj.SetActive(true);
        activeCanvas.SetActive(true);
    }
}
