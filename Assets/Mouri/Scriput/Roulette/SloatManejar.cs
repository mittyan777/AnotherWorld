using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloatManejar : MonoBehaviour
{
    [SerializeField] NewPlayer player;

    // これは数字が決まった瞬間に呼ばれる想定
    public void OnSlotFinished()
    {
        Debug.Log("スロット終了！UIを閉じて操作再開");
        player.Slotstop();
    }
}