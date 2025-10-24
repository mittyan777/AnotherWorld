using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerAtackBase : MonoBehaviour
{
    [SerializeField] Animator _animator;

    /// <summary> /// レンジ構造体 /// </summary>
    struct PlayerRange
    {
        [SerializeField] int mangicMaxRange;
        [SerializeField] int archerMaxRange;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Avoidance();
    }

    /// <summary> /// 各職種の特殊攻撃 /// </summary>
    protected virtual void Special_Attack()
    {
        //各職種の特殊攻撃
    }

    /// <summary> s/// 全職種共通の攻撃処理 /// </summary>
    protected virtual void Atack_Normal()
    {
        //TODO:マウス左クリックで通常攻撃
        if (Input.GetMouseButtonDown(0))
        {
            //攻撃のダメージ処理
            
            //TODO：攻撃アニメーション

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
            _animator.SetBool("avoidance", true);
            Debug.Log("回避方向");
        }
        else
        {
            _animator.SetBool("avoidance", false);
        }
        
        
    }



}
