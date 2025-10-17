using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer1 : MonoBehaviour  //�v���C���[�̎��E�̌��E�l��ARay�̎��o�� //�v���C���[������ł��邩�ł��Ȃ����𔻒f���镔��
{

    //Animator animator;
    [SerializeField]
    public float moveSpeed = 5f;
    public float coin = 0;
    public bool canControl = true;  // �� �ǉ��I(�v���C���[����������邩���Ȃ����̔��f�ꏊ�j
    //[SerializeField] GameObject RayPointer;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();  �A�j���[�V������ǉ�����Ƃ��̓R�����g�A�E�g���O��
        // Cursor.lockState = CursorLockMode.Locked;   //�ǉ��iesc�������ƃ}�E�X�J�[�\�����Ăяo���j
        //Cursor.visible = false;     //�ǉ�
    }

    // Update is called once per frame
    void Update()
    {
        //RayPointer.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // �����ő���S���~�܂�




        // �O����Ray���΂��āA���[���b�g��ɓ���������
        //Ray ray = new Ray(new Vector3(RayPointer.transform.position.x, RayPointer.transform.position.y, RayPointer.transform.position.z), RayPointer.transform.forward);
        //RaycastHit hit;



        //if (Physics.Raycast(ray, out hit, Direction)) //�iDirection�j��Ray�̋�����ݒ�ł���
        //{
        //    if (hit.collider.CompareTag("Roulette")) // ���[���b�g��̃^�O��"Roulette"�ɂ���
        //    {
        //        // �uE�v�L�[�Ń��[���b�g�𒲂ׂ�
        //        if (Input.GetKeyDown(KeyCode.E))
        //        {
        //            Debug.Log("���[���b�g�𒲂ׂ��I�V�[���؂�ւ�");
        //            slot.SetActive(true);
        //            //RouletteUIManager.Instance.OpenRouletteUI(this);

        //            canControl = false; //�ǉ��i�����~�������邩�����Ȃ����𔻒f����j

        //            var cameraController = Camera.main.GetComponent<MainCamera>();
        //            if (cameraController != null)
        //            {
        //                cameraController.enabled = false;
        //            }
        //        }

        //    }
        //}
        //Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);

    }
    private void FixedUpdate()
    {

        
        if (!canControl) return;        //�ǉ��i����֎~���𔻒f���Ă���j
        Move();


        /*if (Input.GetKey("w"))
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
        else { animator.SetBool("right", false); }*/


    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }

    public void Slotstop()      //�ǉ������@�X���b�g���Ńv���O�������Ăяo��
    {
        canControl = true;
        slot.SetActive(false);

        var cameraController = Camera.main.GetComponent<MainCamera>();
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
    }
}
