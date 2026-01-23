using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName =("ArcherAimWalkSO"),menuName =("ArcherAimWalkSO"))]
public class ArcherAimWalkSO : AnimationBaseSO
{
    //ADS時のプレイヤースピード
    [SerializeField] private float adsMoveSpeed = 2.5f;

    //各bool値のパラメータネーム
    private const string PARAM_FWD           = "IsMovingFwd";
    private const string PARAM_BACK          = "IsMovingBack";
    private const string PARAM_RIGHT         = "IsMovingRight";
    private const string PARAM_LEFT          = "IsMovingLeft";
    private const string PARAM_DRAW_TRIGGER  = "StartADS";
    private const string PARAM_SHOOT_TRIGGER = "Recoil";

    private const float INPUT_THRESHOLD = 0.1f;
    private const float MOVE_THRESHOLD = 0.01f;

    public override void Execute(Animator animator)
    {
        UnityEngine.Assertions.Assert.IsFalse(
         true,
         "移動コマンド Execute が誤って呼び出し。ExecuteMovement() を使用して。");
        return;
    }

    public void ExecuteMoveMent(Animator animator, Transform transform, Vector2 moveInput,
        bool isAimingStatFrame,bool isShootRequested)
    {

        //構えアニメーションの実行
        if (isAimingStatFrame) 
        {
            animator.SetTrigger(PARAM_DRAW_TRIGGER);
            return;
        }

        //発射アニメーションの実行
        if (isShootRequested) 
        {
            int adsLayerIndex = animator.GetLayerIndex("ADS Layer");
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(adsLayerIndex);

            if (!currentState.IsName("Standing Aim Recoil 1"))
            {
                animator.SetTrigger(PARAM_SHOOT_TRIGGER);
            }
            return;
        }

        //移動処理
        Vector3 _moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        if (_moveDirection.magnitude > 1.0f) { _moveDirection.Normalize(); }
        transform.position += _moveDirection * adsMoveSpeed * Time.deltaTime;

        Vector3 inputVector = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 localInput = Quaternion.Inverse(transform.rotation) * inputVector;

        //前後移動
        bool isMovingFwd  = localInput.z > INPUT_THRESHOLD;  
        bool isMovingBack = localInput.z < -INPUT_THRESHOLD;

        //左右移動
        bool isMovingRight = localInput.x > INPUT_THRESHOLD;
        bool isMovingLeft  = localInput.x < -INPUT_THRESHOLD;

        //アニメーションフラグセット
        animator.SetBool(PARAM_FWD, isMovingFwd);
        animator.SetBool(PARAM_BACK, isMovingBack);
        animator.SetBool(PARAM_RIGHT, isMovingRight);
        animator.SetBool(PARAM_LEFT, isMovingLeft);

    }
}
