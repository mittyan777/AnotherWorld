using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BossManager : MonoBehaviour
{
    //staticを使用し、いつでも他スクリプトから情報を見れるようにする
    public static BossManager instance { get; private set; }

    [SerializeField] private GameObject playerObj;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        { 
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        LookPlayer();
    }

    //この関数を呼び出したScriptのリストにあるfloat型のデータをリストで取得
    public int GetActionIndex(List<float> weights)
    {
        //リスト自体があるかチェック
        if (weights == null || weights == null)
        {
            return -1;
        }

        //リスト内にデータがあるかチェック
        float totalWeight = 0;
        for (int i = 0; i < weights.Count; i++)
        { 
            totalWeight += weights[i];
        }
        
        //行動の確率が0より大きいかチェック
        if (totalWeight <= 0)
        {
            return -1;
        }

        //ランダムで行動を選抜
        float drawValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < weights.Count; i++)
        { 
            currentWeight += weights[i];

            if (drawValue < currentWeight)
            { 
                return i;
            }
        }
        return 0;
    }

    private void LookPlayer() 
    {
        transform.LookAt(playerObj.transform);
    }
}
