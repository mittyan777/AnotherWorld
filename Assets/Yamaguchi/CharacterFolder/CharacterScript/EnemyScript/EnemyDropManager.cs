using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDropManager : MonoBehaviour
{
    //[SerializeField] private GameObject coinObj;
    [SerializeField] private GameObject moneyObj;
    //確率指定
    [SerializeField][Range(0f, 100f)] private float recoveryDrop;
    [SerializeField] private GameObject recoveryObj;

    public void DropItem()
    {
        //経験値ドロップ
        CoinDrop();        
    }

    private void CoinDrop()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= recoveryDrop)
        {
            Instantiate(recoveryObj, transform.position, Quaternion.identity);
            Debug.Log("回復をドロップ成功");
        }
        else 
        {
            Debug.Log("回復をドロップ失敗");
        }
        
        //お金のドロップ
        Instantiate(moneyObj, transform.position, Quaternion.identity);
    }
}
