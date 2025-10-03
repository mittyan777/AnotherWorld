using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 currentPos; // ���݂̃J�����ʒu
    Vector3 pastPos;    // �ߋ��̃J�����ʒu
    Vector3 diff;       // �ړ�����

    // �J������]�p
    float verticalAngle = 0f; // �c��]�p�x��ێ��i�����ŊǗ�����j
    [SerializeField] float minVertical = -30f; // ����������ŏ��p�x
    [SerializeField] float maxVertical = 30f;  // ���������ő�p�x
    [SerializeField] float sensitivity = 1f;   // �}�E�X���x

    private void Start()
    {
        // �ŏ��̃v���C���[�̈ʒu���L�^
        pastPos = player.transform.position;
    }

    void Update()
    {
        // ------ �J�����̈ړ� ------
        currentPos = player.transform.position;
        diff = currentPos - pastPos;
        transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);
        pastPos = currentPos;

        // ------ �J�����̉�] ------
        float mx = Input.GetAxis("Mouse X") * sensitivity;
        float my = Input.GetAxis("Mouse Y") * sensitivity;

        // ����]�i�v���C���[�̎��͂���]�j
        if (Mathf.Abs(mx) > 0.01f)
        {
            transform.RotateAround(player.transform.position, Vector3.up, mx);
        }

        // �c��]�i�p�x��������j
        if (Mathf.Abs(my) > 0.01f)
        {
            verticalAngle += -my; // �㉺�͋t�Ȃ̂Ń}�C�i�X��t����
            verticalAngle = Mathf.Clamp(verticalAngle, minVertical, maxVertical);

            // ���݂̃J�����̊p�x����A���䂵����X����]�����ݒ�
            Vector3 euler = transform.eulerAngles;
            euler.x = verticalAngle;
            transform.eulerAngles = new Vector3(euler.x, transform.eulerAngles.y, 0);
        }

        // �v���C���[�̌����̓J������Y�������f
        player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
