using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SendSlashEffect : MonoBehaviour
{
    [Header("スラッシュエフェクト設定")]
    [SerializeField] private float slashspeed = 30.0f;
    [SerializeField] private float effectlifeTime = 0.5f;

    void Start()
    {
        Destroy(this.gameObject, effectlifeTime);
    }

    void FixedUpdate()
    {
        transform.position = this.transform.position * slashspeed * Time.deltaTime ;
    }

    /// <summary> /// 斬撃は貫通なのでトリガー使用 /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy") 
        {
            //TODO:ダメージ処理

            //ダメージ処理未作成のためデバッグログ
            UnityEngine.Debug.Log("接触判定");
        }
        Destroy(this.gameObject);
    }


}
