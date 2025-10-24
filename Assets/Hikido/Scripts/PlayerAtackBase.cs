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

    /// <summary> /// �e�E��̓���U�� /// </summary>
    protected virtual void Special_Attack()
    {
        //�e�E��̓���U��
    }

    /// <summary> s/// �S�E�틤�ʂ̍U������ /// </summary>
    private void Atack_Normal()
    {
        //TODO:�}�E�X���N���b�N�Œʏ�U��
        if (Input.GetMouseButtonDown(0))
        {
            //�U���̃_���[�W����

            //TODO�F�A�j���[�V��������
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

        UnityEngine.Vector3 _inputDirection = new UnityEngine.Vector3(_inputHorizontal, 0f, _inputVertical).normalized;

        //Shift + �����L�[�ł̉��
        if(_isShiftKey  && _inputDirection.magnitude > 0.1f)
        {
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(_inputDirection);
            transform.rotation = targetRotation;
        }

        //TODO:����A�j���[�V����(�A�j���[�^�[�����j
        
    }



}
