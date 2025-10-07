using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_mitubosi : MonoBehaviour
{
    Animator animator;  
    public float coin = 0;
    public bool canControl = true;  // �� �ǉ��I(�v���C���[����������邩���Ȃ����̔��f�ꏊ�j
    [SerializeField] GameObject rayobj;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
       // Cursor.lockState = CursorLockMode.Locked;   //�ǉ��iesc�������ƃ}�E�X�J�[�\�����Ăяo���j
        //Cursor.visible = false;     //�ǉ�
    }

    // Update is called once per frame
    void Update()
    {
       rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // �����ő���S���~�܂�


      
        
            // �O����Ray���΂��āA���[���b�g��ɓ���������
            Ray ray = new Ray(new Vector3(rayobj.transform.position.x, rayobj.transform.position.y, rayobj.transform.position.z ), rayobj.transform.forward);
            RaycastHit hit;



            if (Physics.Raycast(ray, out hit, Direction)) // 2m�ȓ��𒲂ׂ�
            {
                if (hit.collider.CompareTag("Roulette")) // ���[���b�g��̃^�O��"Roulette"�ɂ���
                {
                    // �uE�v�L�[�Ń��[���b�g�𒲂ׂ�
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("���[���b�g�𒲂ׂ��I�V�[���؂�ւ�");
                        slot.SetActive(true);

                        canControl = false; //�ǉ��i�����~�������邩�����Ȃ����𔻒f����j

                        //RouletteUIManager.Instance.OpenRouletteUI(this);
                        var cameraController = Camera.main.GetComponent<PlayerCamera>();
                        if (cameraController != null)
                        {
                            cameraController.enabled = false;
                        }
                    }

                }
            }
            Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);
        
    }
    private void FixedUpdate()
    {
        if (!canControl) return;        //�ǉ��i����֎~���𔻒f���Ă���j

        if(Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            animator.SetBool("walk", true);
        }
        else { animator.SetBool("walk", false); }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * 5 * Time.deltaTime;
            animator.SetBool("back", true);
        }
        else { animator.SetBool("back", false); }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * 5 * Time.deltaTime;
            animator.SetBool("left", true);
        }
        else { animator.SetBool("left", false); }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            animator.SetBool("right", true);
        }
        else { animator.SetBool("right", false); }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }

    public void Slotstop()      //�ǉ������X���b�g���Ńv���O�������Ăяo��
    {
        canControl = true;
        slot.SetActive(false);

        var cameraController = Camera.main.GetComponent<PlayerCamera>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
    }
}
