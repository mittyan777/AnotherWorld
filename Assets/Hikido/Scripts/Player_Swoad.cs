using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Player_Swoad : PlayerAtackBase
{
    //攻撃カウントの定数
    private const float ATTACKCOUNT = 0.5f;

    [Header("コンボ用")]
    [SerializeField] float inputwaitTime = 0.5f;
    [SerializeField] int maxComboCount = 3;

    private float _comboTime = 0.0f;
    private int _comboCount = 0;
    private bool isAttack = false;

    void Start()
    {
        inputwaitTime = ATTACKCOUNT;
       
    }

    protected override void Update()
    {
        base.Update();
        //コンボ時間減算
        ComboTime();

        //TODO;通常攻撃判定後　＋　剣士だった場合のみ
        if (base.bNormalAttack ==  true)
        {
            //剣士特殊攻撃
            Special_Attack();
        }
      

    }

    protected override void Special_Attack()
    {
        //剣士だった場合のみー＞　通常攻撃後
        SwordSkill();
    }

    private void SwordSkill()
    {
        //左クリック入力状況を保持
        bool _bisAttack = Input.GetMouseButtonDown(0);
        

        if (_bisAttack ) 
        {
            if (_comboCount == 0) { StartCombo(); }
            else if (_comboTime > 0.0f) { NextCombo(); }
        }
      
    }


    private void StartCombo()
    {
        //攻撃中なら無視
        if (isAttack) return;
        //TODO：実際の攻撃処理 -> 攻撃アニメーションは別クラスでまとめて行うので後に
        isAttack = true;
        _comboTime += inputwaitTime;
        _comboCount++;
        //攻撃処理
        //TODO:アニメーションがないため確認用でデバッグログ出力
        ComboAnimation(_comboCount);
        Debug.Log("コンボ確認");
        
    }

    //コンボ攻撃時のタイマー
    private void ComboTime()
    {
        if (_comboTime > 0.0f)
        {
            _comboTime -= Time.deltaTime;
        }
        if (_comboTime <= 0.0f && _comboCount > 0)
        {
            ComboReset();
            Debug.Log("リセット処理");
        }
    }

    private void NextCombo() 
    {
        //TODO:次のコンボ
        if(_comboCount < maxComboCount)
        {
            _comboCount++;
            _comboTime = inputwaitTime;
            ComboAnimation(_comboCount);
            Debug.Log("2コンボ目の確認");
        }
    }

    //コンボリセット
    private void ComboReset()
    {
        //TODO:コンボのリセット処理
        //Debug.Log("コンボリセット確認");
        isAttack = false;
        _comboTime = 0.0f;
        _comboCount = 0;
    }


    //コンボのアニメーション
    private void ComboAnimation(int _comboCount) 
    {
        Debug.Log("アニメーション処理確認");
        //TODO;アニメーションクラスの処理を呼び出す。

        //アニメーションクラスからアニメーションのクリップ時間を取得する
        float _animationLength = 0.5f;

        //アニメーションの間隔
        AttackDuration(_animationLength);

    }

    //攻撃アニメーション間隔
    IEnumerator AttackDuration(float _combo)
    {
        //アニメーションクラスでアニメーションのクリップ時間を取得する。
        yield return new WaitForSeconds(_combo);
        //isAttack = false;

    }
}
