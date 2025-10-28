using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerAtackBase : MonoBehaviour
{
    Animator _animator;
   


    void Start()
    {
        
    }

    void Update()
    {
       
    }

    /// <summary> /// 各職種の特殊攻撃 /// </summary>
    protected virtual void Special_Attack()
    {
        //各職種の特殊攻撃
    }

    /// <summary> s/// 全職種共通の攻撃処理 /// </summary>
    private void Atack_Normal()
    {
        //TODO:マウス左クリックで通常攻撃
        if (Input.GetMouseButtonDown(0))
        {
            //攻撃のダメージ処理

            //TODO：アニメーション処理
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

        UnityEngine.Vector3 _inputDirection = new UnityEngine.Vector3(_inputHorizontal, 0f, _inputVertical).normalized;

        //Shift + 方向キーでの回避
        if(_isShiftKey  && _inputDirection.magnitude > 0.1f)
        {
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(_inputDirection);
            transform.rotation = targetRotation;
        }

        //TODO:回避アニメーション(アニメーター処理）
        
    }



}
