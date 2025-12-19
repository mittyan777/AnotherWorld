using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] private int maxMoneyAmount;
    [SerializeField] private int minMoneyAmount;
    public int money;

    private void Start()
    {
        int randomValue = Random.Range(minMoneyAmount, maxMoneyAmount);
        money = randomValue;

        //Debug.Log(money);
    }
}
