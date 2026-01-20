using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    // コインの上下移動
    [SerializeField] private float maxCoinPosY;
    [SerializeField] private float minCoinPosY;
    [SerializeField] private float coinMoveSpeed;

    //コインの回転速度
    [SerializeField] private float rotationalSpeed;

    // 内部変数
    private Vector3 initialPosition;
    private bool upFlag = true;

    private void Start()
    {
        //コインの初期値保存
        initialPosition = transform.position;
    }

    private void Update()
    {
        UpAndDownMove();

        transform.Rotate(Vector3.up, rotationalSpeed * Time.deltaTime);
    }

    private void UpAndDownMove()
    {

        float currentRelativeY = transform.position.y - initialPosition.y;
        float moveAmount = coinMoveSpeed * Time.deltaTime; // 移動量を計算

        //移動方向決め
        if (upFlag)
        {
            transform.position += Vector3.up * moveAmount;
            if (currentRelativeY >= maxCoinPosY)
            {
                upFlag = false;
            }
        }
        else
        {
            transform.position += Vector3.down * moveAmount;
            if (currentRelativeY <= minCoinPosY)
            {
                upFlag = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            Destroy(gameObject);
        }
    }
}