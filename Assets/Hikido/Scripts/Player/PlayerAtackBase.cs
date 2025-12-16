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
    
    [SerializeField] protected AnimationFlagManagerSO _animflgSO;

    void Start()
    {
        _animflgSO.Avoidflg = false;
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
        bool _inputAtk = Input.GetMouseButtonDown(0);
        if (_inputAtk)
        {
           // _animflgSO.MajicAttackNormalFlg = true;
        }

    }

    //TODO:マウスで向いている方向に回避する。
    /// <summary> /// 全職種共通の回避 /// </summary>
    private void Avoidance()
    {
        //入力数値取得
        float _inputHorizontal = Input.GetAxisRaw("Horizontal");
        float _inputVertical = Input.GetAxisRaw("Vertical");

        //shiftキー入力判定
        bool _isShiftKey = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        bool _isSpaceKey = Input.GetKeyDown(KeyCode.Space);

        Transform _cameraTransform = Camera.main.transform;

        UnityEngine.Vector3 _cameraForward = UnityEngine.Vector3.Scale(_cameraTransform.forward, new UnityEngine.Vector3(1, 0, 1)).normalized;
        UnityEngine.Vector3 _cameraRight = UnityEngine.Vector3.Scale(_cameraTransform.right, new UnityEngine.Vector3(1, 0, 1)).normalized;

        UnityEngine.Vector3 _inputDirection = (_cameraForward * _inputVertical) + (_cameraRight * _inputHorizontal);
        _inputDirection = _inputDirection.normalized;

        //Shift + 方向キーでの回避
        if (_isShiftKey && _inputDirection.magnitude > 0.1f || _isSpaceKey && _inputDirection.magnitude > 0.1f)
        {
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(_inputDirection);
            transform.rotation = targetRotation;

            //回避アニメーションフラグ
            _animflgSO.Avoidflg = true;
            Debug.Log("回避方向");
        }
        else { _animflgSO.Avoidflg = false; }
     
    }
}
