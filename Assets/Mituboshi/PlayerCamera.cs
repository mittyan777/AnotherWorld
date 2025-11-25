using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 currentPos; // 現在のカメラ位置
    Vector3 pastPos;    // 過去のカメラ位置
    Vector3 diff;       // 移動距離
    [SerializeField] GameObject AdsPos;
    [SerializeField] GameObject NO_ADS;
    [SerializeField]float distance;
    [SerializeField] float distance2;
    [SerializeField]bool ADS_reset = false;
    [SerializeField]float ADS_reset_Time;
    public bool ADS = false;

    // カメラ回転用
    float verticalAngle = 0f; // 縦回転角度を保持（自分で管理する）
    [SerializeField] float minVertical = -30f; // 下を向ける最小角度
    [SerializeField] float maxVertical = 30f;  // 上を向ける最大角度
    [SerializeField] float sensitivity = 1f;   // マウス感度

    private void Start()
    {
        // 最初のプレイヤーの位置を記録
        pastPos = player.transform.position;
    }

    void Update()
    {
         distance = Vector3.Distance(AdsPos.transform.position, transform.position);
        distance2 = Vector3.Distance(NO_ADS.transform.position, transform.position);
        player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0) ;
            // ------ カメラの移動 ------
            currentPos = player.transform.position;
            diff = currentPos - pastPos;
            transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);
            pastPos = currentPos;
            // ------ カメラの回転 ------
            float mx = Input.GetAxis("Mouse X") * sensitivity;
            float my = Input.GetAxis("Mouse Y") * sensitivity;
        mx = Mathf.Clamp(mx, -70, 70);
        // 横回転（プレイヤーの周囲を回転）
            if (Mathf.Abs(mx) > 0.01f)
            {
                transform.RotateAround(player.transform.position, Vector3.up, mx);
            }

            // 縦回転（角度制限あり）
            if (Mathf.Abs(my) > 0.01f)
            {
                verticalAngle += -my; // 上下は逆なのでマイナスを付ける
                verticalAngle = Mathf.Clamp(verticalAngle, minVertical, maxVertical);

                // 現在のカメラの角度から、制御したいX軸回転だけ設定
                Vector3 euler = transform.eulerAngles;
                euler.x = verticalAngle;
                transform.eulerAngles = new Vector3(euler.x, transform.eulerAngles.y, 0);
            }
        
        if(Input.GetMouseButton(1))
        {
            ADS = true;
            transform.position = AdsPos.transform.position;
            // オブジェクトを移動
            if (distance >= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, AdsPos.transform.position, 10 * Time.deltaTime);
                //transform.position += direction * 10 * Time.deltaTime;
            }


        }
        if (Input.GetMouseButtonUp(1))
        {
            transform.position = NO_ADS.transform.position;
            ADS_reset = true;
            ADS = false;

        }
      
    }
}
