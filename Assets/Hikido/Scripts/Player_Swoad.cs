using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Swoad : PlayerAtackBase
{
    //攻撃カウントの定数
    private const float ATTACKCOUNT = 10.0f;

    [Header("コンボ用")]
    [SerializeField] float inputwaitTime = 0.5f;
    [SerializeField] int maxComboCount = 3;

    private float _comboTime = 0.0f;

    bool isAttack = false;
    bool isAttackEnd = false;

    void Start()
    {
        inputwaitTime = ATTACKCOUNT;
    }

    void Update()
    {
        //コンボ時間減算
        ComboTime();

        //剣士特殊攻撃
        Special_Attack();
    }

    protected override void Special_Attack()
    {
        //剣士だった場合のみ + 攻撃中で判断
        isAttack = true;  
        SwordSkill();
    }

    private void SwordSkill()
    {
        //左クリック入力状況を保持
        bool _bisAttack = Input.GetMouseButton(0);

        if(_bisAttack) { StartCombo(); }
        else if(_comboTime > 0.0f) { NextCombo(); }
    }


    private void StartCombo()
    {
        //TODO：実際の攻撃処理 -> 攻撃アニメーションは別クラスでまとめて行うので後に

        
    }

    private void NextCombo() 
    {
        //TODO:次のコンボ
    }

    //コンボ攻撃時のタイマー
    private void ComboTime()
    { 
        if(_comboTime < 0.0f) 
        {
            _comboTime -= Time.deltaTime;
        }
        if(_comboTime <= 0.0f) 
        {
            ComboReset();
        }
    }

    //コンボリセット
    private void ComboReset()
    {
        //TODO:コンボのリセット処理
    }

}
