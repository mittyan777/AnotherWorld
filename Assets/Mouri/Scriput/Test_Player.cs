using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;


    private void Start()
    {

        if (!CompareTag("Player"))
        {
            Debug.LogWarning("Playerタグが付いていないため、このスクリプトは無効化されました。");
            enabled = false;
        }

    }


    private void Update()
    {

        Move();

    }


    private void Move()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(x, 0f, z);

        if (moveDirection == Vector3.zero)
        {
            return;
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

    }

}

