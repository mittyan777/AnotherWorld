using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Achel: MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // ��̃v���n�u
    [SerializeField] private Transform shootPoint;   // ��̔��ˈʒu�i�|�̐�[�Ȃǁj
    [SerializeField] private float shootForce = 25f; // ��̔�ԗ�

    void Update()
    {
        // ���N���b�N�i�}�E�X�{�^��0�j�Ŕ���
        if (Input.GetMouseButtonDown(0))
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        // ��𐶐�
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Rigidbody�����Ă���ΑO���ɗ͂�������
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }

        // 10�b��Ɏ����폜�i����������Ȃ��悤�Ɂj
        Destroy(arrow, 10f);
    }
}

