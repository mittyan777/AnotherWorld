using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerAtackBase : MonoBehaviour
{
    [SerializeField] Animator _animator;

    /// <summary> /// �����W�\���� /// </summary>
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

    /// <summary> /// �e�E��̓���U�� /// </summary>
    protected virtual void Special_Attack()
    {
        //�e�E��̓���U��
    }

    /// <summary> s/// �S�E�틤�ʂ̍U������ /// </summary>
    protected virtual void Atack_Normal()
    {
        //TODO:�}�E�X���N���b�N�Œʏ�U��
        if (Input.GetMouseButtonDown(0))
        {
            //�U���̃_���[�W����
            
            //TODO�F�U���A�j���[�V����

        }
        
    }

    /// <summary> /// �S�E�틤�ʂ̉�� /// </summary>
    private void Avoidance()
    {
        //���͐��l�擾
        float _inputHorizontal = Input.GetAxisRaw("Horizontal");
        float _inputVertical = Input.GetAxisRaw("Vertical");

        //shift�L�[���͔���
        bool _isShiftKey = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        bool _isSpaceKey = Input.GetKeyDown(KeyCode.Space);

        UnityEngine.Vector3 _inputDirection = new UnityEngine.Vector3(_inputHorizontal, 0f, _inputVertical).normalized;

        //Shift + �����L�[�ł̉��
        if (_isShiftKey && _inputDirection.magnitude > 0.1f || _isSpaceKey && _inputDirection.magnitude > 0.1f)
        {
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(_inputDirection);
            transform.rotation = targetRotation;
            _animator.SetBool("avoidance", true);
            Debug.Log("������");
        }
        else
        {
            _animator.SetBool("avoidance", false);
        }
        
        
    }



}
