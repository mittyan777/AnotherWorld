using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveeeeeeee : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ肷�鍀��
    [Header("�ړ��ݒ�")]
    [Tooltip("�v���C���[�̈ړ����x")]
    public float moveSpeed = 5.0f;
    [Tooltip("��]���x�i�J�����̕����Ɍ��������j")]
    public float rotationSpeed = 10.0f;

    // �J������Transform (TPS�J�����A�܂��̓��C���J����)
    [Header("�J�����A�g")]
    [Tooltip("�J�����I�u�W�F�N�g��Transform���A�^�b�`")]
    public Transform cameraTransform;

    private CharacterController controller; // �v���C���[�̈ړ��Ɏg�p����R���|�[�l���g
    private Vector3 moveDirection;          // �v�Z���ꂽ�ړ�����

    void Start()
    {
        // CharacterController�R���|�[�l���g���擾
        controller = GetComponent<CharacterController>();

        // CharacterController���Ȃ��ꍇ�͌x�����o���ăX�N���v�g�𖳌���
        if (controller == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController component on the same GameObject.");
            enabled = false;
        }

        // cameraTransform�����ݒ�̏ꍇ�́A���C���J������T���Đݒ�
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("Main Camera not found. Please set the Camera Transform manually.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        // 1. ���͂̎擾
        float horizontal = Input.GetAxis("Horizontal"); // A/D�܂��͍��X�e�B�b�NX
        float vertical = Input.GetAxis("Vertical");   // W/S�܂��͍��X�e�B�b�NY

        // 2. �ړ������̌v�Z
        // ���̓x�N�g�����쐬
        Vector3 inputVector = new Vector3(horizontal, 0f, vertical).normalized;

        // ���͂��Ȃ���Ώ������I��
        if (inputVector.magnitude < 0.1f)
        {
            // �ړ����~���A�d�͏����̂ݍs��
            moveDirection.x = 0;
            moveDirection.z = 0;

            // �d�͓K�p�iCharacterController�͎����ŏd�͂������Ȃ����߁A�����œK�p���K�v�j
            ApplyGravity();

            // �Ō�ɃR���g���[���[�ňړ�
            controller.Move(moveDirection * Time.deltaTime);
            return;
        }

        // 3. �J�����̌����Ɋ�Â����ړ������̕ϊ�
        // �J������Y����]�݂̂��擾�i�㉺�̌X���͖����j
        Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // ���͕������J�����̉�]�ɍ��킹�ă��[���h���W�n�ɕϊ�
        Vector3 desiredMoveDirection = cameraRotation * inputVector;

        // 4. ��]���� (�v���C���[��i�s�����Ɍ�����)
        Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);

        // ���炩�ɉ�]
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 5. �ŏI�I�Ȉړ�������ݒ�
        // Y���̑��x�i�d�͂Ȃǁj��ێ������܂܁A���������̑��x���X�V
        float ySpeed = moveDirection.y;
        moveDirection = desiredMoveDirection * moveSpeed;
        moveDirection.y = ySpeed; // Y�����x��߂�

        // 6. �d�͓K�p
        ApplyGravity();

        // 7. ���ۂ̈ړ����s
        controller.Move(moveDirection * Time.deltaTime);
    }

    // �d�͏������J�v�Z����
    void ApplyGravity()
    {
        // �v���C���[���n�ʂɐڒn���Ă��Ȃ��ꍇ�A�d�͂�K�p
        if (!controller.isGrounded)
        {
            // �ȒP�ȏd�͉����x�̓K�p (��: -9.8f)
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // �n�ʂɐڒn���Ă���ꍇ�AY���̑��x�����Z�b�g�i�킸���ɒn�ʂɉ����t����l�ɂ��Ă����j
            if (moveDirection.y < 0)
            {
                moveDirection.y = -2f;
            }
        }
    }
}
