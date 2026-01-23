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
    Transform _cameraTransform;

    [SerializeField] protected AnimationFlagManagerSO _animflgSO;
    [SerializeField] private Collider playerCollider;

    void Start()
    {
        if (playerCollider == null) playerCollider = GetComponent<Collider>();
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

    public void DisableCollider()
    {
        if (playerCollider != null)
        {
            playerCollider.isTrigger = true;
            Debug.Log("コライダーOFF：無敵状態");
        }
    }

    public void EnableCollider()
    {
        if (playerCollider != null)
        {
            playerCollider.isTrigger = false;
            _animflgSO.Avoidflg = false;
            Debug.Log("コライダーON：通常状態");
        }
    }

    //TODO:マウスで向いている方向に回避する。
    /// <summary> /// 全職種共通の回避 /// </summary>
    private void Avoidance()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool _isShiftKey = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        bool _isSpaceKey = Input.GetKeyDown(KeyCode.Space);

        // 回避キーが押された瞬間だけ判定
        if (_isShiftKey || _isSpaceKey)
        {
            // 入力がある場合のみ実行
            if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
            {
                float targetAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
                float materialOffset = 0f;

                transform.rotation = UnityEngine.Quaternion.Euler(0, targetAngle + materialOffset, 0);

                _animflgSO.Avoidflg = true;
                Debug.Log($"入力方向 {targetAngle} 度へTransformを固定して回避開始");
            }
        }

    }
}
