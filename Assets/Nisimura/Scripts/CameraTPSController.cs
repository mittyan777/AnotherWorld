using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTPSController : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ肷�鍀��
    [Header("�^�[�Q�b�g�ݒ�")]
    [Tooltip("�J�������Ǐ]���A���S�ƂȂ��ĉ�]����Ώہi�v���C���[�j")]
    public Transform targetplayer;

    [Header("�J�����ݒ�")]
    [Tooltip("�^�[�Q�b�g����̋���")]
    public float distance = 5.0f;
    [Tooltip("���������̎��_�����i�ŏ��p�x�j")]
    public float minYAngle = -10.0f;
    [Tooltip("���������̎��_�����i�ő�p�x�j")]
    public float maxYAngle = 80.0f;

    [Header("���x�ݒ�")]
    [Tooltip("���������̊��x")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("���������̊��x")]
    public float mouseSensitivityY = 2.0f;

    private float currentX = 0.0f; // ���������̗ݐω�]�p�x
    private float currentY = 0.0f; // ���������̗ݐω�]�p�x

    void Start()
    {
        // �J�[�\�������b�N���Ĕ�\���ɂ���
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // �}�E�X�̈ړ��ʂ��擾
        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY; // Y���͏㉺���]�����邽�߃}�C�i�X

        // ���������̉�]�p�x�𐧌�
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (targetplayer == null) return;

        // 1. ��]���v�Z
        // �ݐς����p�x����J�����̉�]�iQuaternion�j���쐬
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 2. �ʒu���v�Z
        // �^�[�Q�b�g�̈ʒu����A��]���l�����w�苗���������ꂽ�ʒu���v�Z
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + targetplayer.position;

        // 3. �J�����ɓK�p
        transform.rotation = rotation;
        transform.position = position;
    }
}
