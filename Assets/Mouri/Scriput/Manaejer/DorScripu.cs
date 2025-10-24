using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorScripu : MonoBehaviour
{
    [SerializeField] private Transform player;      // プレイヤーのTransform
    [SerializeField] private Animator doorAnimator; // ドアのAnimator
    [SerializeField] private float openDistance ; // 開く距離
    [SerializeField] private Transform DistancePointer;//ドアのポジションで計算してしまうとずれが生じるため空のオブジェクトを利用してドアの計算を正しく処理するための部分

    [SerializeField] float distance;
    private bool isLocked = false;
   // private bool isPlayerRange = false;


    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        // "Player" タグのオブジェクトを自動で探して取得
        GameObject obj = GameObject.FindGameObjectWithTag("Player");//FindGameObjetWithTagという文字はタグをつけているこのゲームオブジェクトを探してという意味
        if (obj != null)
        {
            player = obj.transform;
        }
        else
        {
            Debug.LogError("Playerタグのオブジェクトが見つかりません！");
        }
    }

    private void Update()
    {
        if (isLocked) return;

         distance = Vector3.Distance(player.position,DistancePointer.position); //Vector3内にある「Distance]を呼び起こしている

        Debug.Log("距離：" + distance);

        if (distance <= openDistance)
        {
           
            doorAnimator.SetBool("open", true);
            Debug.Log("ドアを開く");
        }
        else 
        {
            doorAnimator.SetBool("open", false);
            
        }
    }
    public void LockDoor()
    {
        doorAnimator.SetBool("open", false);
        isLocked = true;
        Debug.Log("ドアをロック");
    }

}
