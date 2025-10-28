using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDropManager : MonoBehaviour
{
    //[SerializeField] private GameObject coinObj;
    [SerializeField] private GameObject moneyObj;
    //確率指定
    //[SerializeField][Range(0f, 100f)] private float coinDrop;

    public void DropItem()
    {
        //経験値ドロップ
        CoinDrop();        
    }

    private void CoinDrop()
    {
        /*
        if (coinObj == null)
        {
            return;
        }
        

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= coinDrop)
        {
            Instantiate(coinObj, transform.position, Quaternion.identity);
            Debug.Log("コインをドロップ成功");
        }
        else 
        {
            Debug.Log("コインをドロップ失敗");
        }
        */
        //お金のドロップ
        Instantiate(moneyObj, transform.position, Quaternion.identity);
    }
}
