using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Swoad : PlayerAtackBase
{
    //攻撃カウントの定数
    private const float ATTACKCOUNT = 0.5f;

    [Header("コンボ用")]
    [SerializeField] private float inputwaitTime = 0.5f;
    [SerializeField] private int maxComboCount = 3;

    //斬撃エフェクト
    [SerializeField] private GameObject SlashEffect;
    //斬撃の発生箇所
    [SerializeField] private Transform SlasshPoint;

    //テスト時のみhikido使用
    //[SerializeField] GameManager _manager;
    [SerializeField] GameManager_hikido _manager;

    [SerializeField] private PlayerAnimation _playerAnim;

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

        //TODO;通常攻撃判定後　＋　剣士だった場合のみ-> gamemanager：テスト時のみhikido使用
        //if (bNormalAttack && _manager.GetComponent<GameManager_hikido>().job == 0)
        //{
        //    //剣士特殊攻撃
        //    Special_Attack();
        //}

    }

    protected override void Special_Attack()
    {
        //SwordSkill();
    }

    public void SwordSkill()
    {
        //左クリック入力状況を保持
        bool _bisAttack = Input.GetMouseButtonDown(0);
        if (_bisAttack ) 
        {
            if (_comboCount == 0) { StartCombo(); }
            else if (_comboTime > 1.0f) { NextCombo(); }
        }
      
    }

    private void StartCombo()
    {
        //攻撃中なら無視
        if (isAttack) return;
        //TODO：実際の攻撃処理 -> 攻撃アニメーションは別クラスでまとめて行うので後に
        isAttack = true;
        _comboTime += inputwaitTime;
        //攻撃処理
        _comboCount++;
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
    public void ComboReset()
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
        if(_playerAnim != null) 
        {
            _playerAnim.AttackAnimation_Swordman(_comboCount);
            _animflgSO.swordAttackAnimFlg = false;
        }
    }

    /// <summary> /// 3コンボ目で斬撃波を生成する /// </summary>
    public void TryAttackCombo()
    {
        if(_comboCount != 2) { return; }

        //発生ポイントの座標を取得
        Quaternion _rotation = SlasshPoint.rotation;

        //斬撃エフェクト生成
        //仮でトランスフォームのポジションから
        Debug.Log("三コンボ目");
        var slashEffectprefa = Instantiate<GameObject>(SlashEffect,SlasshPoint.position,_rotation);
    }
    
}
