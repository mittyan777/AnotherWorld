using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Mouri : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    public bool canControl = true;  // �� �ǉ��I


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("aaaa");
    }

    void Update()
    {
        if (!canControl) return; // �����ő���S���~�܂�

        Move();
        Rotate();

        // �uE�v�L�[�Ń��[���b�g�𒲂ׂ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            // �O����Ray���΂��āA���[���b�g��ɓ���������
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            

            if (Physics.Raycast(ray, out hit, 2f)) // 2m�ȓ��𒲂ׂ�
            {
                if (hit.collider.CompareTag("Roulette")) // ���[���b�g��̃^�O��"Roulette"�ɂ���
                {
                    Debug.Log("���[���b�g�𒲂ׂ��I�V�[���؂�ւ�");
                    //RouletteUIManager.Instance.OpenRouletteUI(this);
                    
                }
            }
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotateSpeed * Time.deltaTime);
    }
}