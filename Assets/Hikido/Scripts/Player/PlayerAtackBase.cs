using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAtackBase : MonoBehaviour
{
    Animator _animator;
    bool bAvoidance = false;
    protected  bool bNormalAttack = false;

    //ゲームマネージャー
    [SerializeField] GameObject[] manager;
    [SerializeField] AnimationFlagManagerSO _animflgSO;

    /// <summary> /// レンジ構造体 /// </summary>
    /// いらないかも
    struct PlayerRange
    {
        [SerializeField] int mangicMaxRange;
        [SerializeField] int archerMaxRange;
    }

    void Start()
    {
        //_animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //回避
        Avoidance();

        //通常攻撃
        Atack_Normal();
    }

    /// <summary> /// 各職種の特殊攻撃 /// </summary>
    protected virtual void Special_Attack()
    {
        //各職種の特殊攻撃 -> 継承先で使用(剣士)
    }

    /// <summary> s/// 全職種共通の攻撃処理 /// </summary>
    protected virtual void Atack_Normal()
    {
        //TODO:通常攻撃の処理(全職種共通の処理)
        bool _inputAtk = Input.GetMouseButtonDown(0);
        if (_inputAtk)
        {
            //TODO:ダメージ処理(全職種共通の処理)
            //->与えるダメージは獲得ステータスの攻撃力分を与える。
            _animflgSO.AttackNormalflg = true;
            bNormalAttack = true;
        }

    }

    /// <summary> /// 全職種共通の回避 /// </summary>
    private void Avoidance()
    {
        //入力数値取得
        float _inputHorizontal = Input.GetAxisRaw("Horizontal");
        float _inputVertical = Input.GetAxisRaw("Vertical");

        //shiftキー入力判定
        bool _isShiftKey = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        bool _isSpaceKey = Input.GetKeyDown(KeyCode.Space);

        UnityEngine.Vector3 _inputDirection = new UnityEngine.Vector3(_inputHorizontal, 0f, _inputVertical).normalized;

        //Shift + 方向キーでの回避
        if (_isShiftKey && _inputDirection.magnitude > 0.1f || _isSpaceKey && _inputDirection.magnitude > 0.1f)
        {
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(_inputDirection);
            transform.rotation = targetRotation;
            bAvoidance = true;
        }
        else
        {
            bAvoidance = false;
        }
        //TODO:回避フラグ必要か？
        if (bAvoidance)
        {
            //TODO:アニメーションフラグで管理
             _animflgSO.Avoidflg = true;
            Debug.Log("回避方向");
        }
    }
    public bool GetbAvoindance()
    {
        return bAvoidance;
    }



}
