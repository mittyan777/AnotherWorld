using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Chest : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider chestTrigger;
    [SerializeField] private KeyCode openKey = KeyCode.E;

    [Header("UI設定")]
    [SerializeField] private GameObject openUI;
    [SerializeField] private float destroyTime = 1.5f;

    private bool isPlayerNear = false;
    private bool isOpened = false;

    private void Reset()
    {

        // 自動取得（入れ忘れ防止）
        animator = GetComponent<Animator>();
        chestTrigger = GetComponent<Collider>();
    }

    private void Start()
    {
        if (openUI != null)
        {
            openUI.SetActive(false);
        }
    }

    private void Update()
    {

        if (!isPlayerNear || isOpened)
        {

            return;
        }

        if (Input.GetKeyDown(openKey))
        {

            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isOpened)   //一度でもあいた場合、プレイヤーが再度当たっても中の処理が一切実行されないようにした。
        {
            return ;
        }

        if (other.CompareTag("Player"))
        {

            Debug.Log("【ENTER】プレイヤーが宝箱に当たった");

            isPlayerNear = true;

            if(openUI != null)
            {
                openUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            Debug.Log("プレイヤーが宝箱の判定から出ました");

            isPlayerNear = false;

            if(openUI!= null)
            {
                openUI.SetActive(false);
            }
        }


    }

    private void OpenChest()
    {

        isOpened = true;
        isPlayerNear = false;

        if(openUI == null)
        {
            openUI.SetActive(false);
        }

        if (animator != null)
        {

            // Open（bool）を OFF にする仕様に対応
            animator.SetBool("open", true);
        }

        if (chestTrigger != null)
        {

            // 一度開いたら判定を無効化
            chestTrigger.enabled = false;
        }

        if(openUI != null)
        {
            openUI.SetActive(false);
        }

        Destroy(gameObject,destroyTime);

        // ここにアイテム取得・SE再生などを後から追加できる
    }
}
