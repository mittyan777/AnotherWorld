using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    // �R�C���̏㉺�ړ�
    [SerializeField] private float maxCoinPosY;
    [SerializeField] private float minCoinPosY;
    [SerializeField] private float coinMoveSpeed;

    //�R�C���̉�]���x
    [SerializeField] private float rotationalSpeed;

    // �����ϐ�
    private Vector3 initialPosition;
    private bool upFlag = true;

    private void Start()
    {
        //�R�C���̏����l�ۑ�
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
        float moveAmount = coinMoveSpeed * Time.deltaTime; // �ړ��ʂ��v�Z

        //�ړ���������
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
}