using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryDrop : MonoBehaviour
{
    [SerializeField] private int maxRecovereAmount;
    [SerializeField] private int minRecovereAmount;
    public int recovereAmount { get; private set; }

    private void Start()
    {
        int randomValue = Random.Range(minRecovereAmount, maxRecovereAmount);
        recovereAmount = randomValue;

        Debug.Log(recovereAmount);
    }
}
