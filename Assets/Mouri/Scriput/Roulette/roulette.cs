using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roulette : MonoBehaviour
{

    //ルーレットのシステム状プログラム     //スロットの数字をランダムで出し、変数に応じた数などを制御している   //ボタンを押した際に画面を消す
    [SerializeField]int power = 0;
    bool powerslot = true;

    [SerializeField] Text powerslot_text;

    [SerializeField] NewPlayer player;

    [SerializeField] GameObject slotUI;

    [SerializeField] int PMIN_Text;

    [SerializeField] int PMAX_Text;

    [SerializeField] float tienn;   //ルーレット画面が消えるまでの時間
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerslot)
        {
            power = Random.Range(PMIN_Text, PMAX_Text);

        }

        powerslot_text.text=power.ToString();
    }
    private void FixedUpdate()
    {
       
        if (powerslot == true) { power = Random.Range(PMIN_Text, PMAX_Text); }
       
        powerslot_text.text = ($"{power}");
    }
    public void stopslot()  //スロットをストップするプログラム
    {
        powerslot = false;

        StartCoroutine(CloseSlotAfterDelay(tienn));     //「StartCoroutine」この関数で時間を数え始める関数（タイマーのスタートボタン？）みたいな役割
    }

    IEnumerator CloseSlotAfterDelay(float delay)        //スタートしている間に、コルーチン（時間を使ったり秒速を数えたりするプログラム）どんな時間のプログラムの中の処理をするかが書いてあり、一時停止や時間のプログラムを再開できるIEn...時間をまたぐ関数
    {
        yield return new WaitForSeconds(delay);     //一時停止をしてもらうプログラム指示       
        slotUI.SetActive(false);
        player.Slotstop();
    }

    public void StartSlot()
    {
        power = 0;
        powerslot = true;
        powerslot_text.text = "0";
        slotUI.SetActive(true);
    }

}
