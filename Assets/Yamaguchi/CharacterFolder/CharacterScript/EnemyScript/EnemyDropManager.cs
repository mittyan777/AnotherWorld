using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDropManager : MonoBehaviour
{
    [SerializeField] private GameObject coinObj;
    //�m���w��
    [SerializeField][Range(0f, 100f)] private float dropChance;

    public void DropItem()
    {
        //�o���l�h���b�v
        CoinDrop();
        
    }

    private void CoinDrop()
    {
        if (coinObj == null)
        {
            return;
        }

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropChance)
        {
            Instantiate(coinObj, transform.position, Quaternion.identity);
            Debug.Log("�R�C�����h���b�v����");
        }
        else 
        {
            Debug.Log("�R�C�����h���b�v���s");
        }
    }
}
