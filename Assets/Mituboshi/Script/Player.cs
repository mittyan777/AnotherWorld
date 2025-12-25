using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    Animator animator;
  
    public float HP;
    public float MP;
    public float AttackStatus;
    public float DefenseStatus;
    public bool canControl = true;  // ← 追加！
    [SerializeField] public bool mouseright = false;
    [SerializeField] bool mouseleft = false;
    [SerializeField] GameObject hand_R;
    [SerializeField] GameObject hand_L;

    [SerializeField] GameObject rayobj;
    [SerializeField] GameObject slot;
    [SerializeField] float Direction;
    [SerializeField] GameObject manager;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject ADSpos;
    [SerializeField] GameObject NOADSpos;

    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField] private float sensitivity = 300f;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField]bool ADS = false;

    [SerializeField]float a = 0.5f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float defaultDistance = 5f; // 通常のカメラ距離
    [SerializeField] private float smoothSpeed = 10f;    // 補間速度
    [SerializeField] private float cameraHeight = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0f;
        animator = GetComponent<Animator>();
       // Cursor.lockState = CursorLockMode.Locked;   //追加
        //Cursor.visible = false;     //追加
    }

    // Update is called once per frame
    void Update()
    {
        if (manager == null) { manager = GameObject.FindGameObjectWithTag("GameManager"); }
        cameracontrol();
        

       
       rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // ここで操作全部止まる


        if (Input.GetKeyDown("p"))
        {
            manager.GetComponent<GameManager>().HP -= 10;
        }
        if(manager.GetComponent<GameManager>().HP <= 0 )
        {
            //Destroy(gameObject);
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha1) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[1].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2 ) 
            {
                manager.GetComponent<GameManager>().gage_image[1].fillAmount = 0;
                GetComponent<Magician_Skills>().FireBall();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }

            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[0].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2 ) 
            {
                manager.GetComponent<GameManager>().gage_image[0].fillAmount = 0;
                GetComponent<Magician_Skills>().ElectricBall();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }
        

        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[2].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2) 
            {
                manager.GetComponent<GameManager>().gage_image[2].fillAmount = 0;
                GetComponent<Magician_Skills>().TornadoAttack();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }
           
        }

        if(manager.GetComponent<GameManager>().job == 1)
        {
            Archercontrol();

        }
      
    }
    private void FixedUpdate()
    {
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
            manager.GetComponent<GameManager>().Coin += 100;
            Destroy(other.gameObject);
        }
    }
    void cameracontrol()
    {
        float mx = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mx;
        xRotation -= my;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (manager.GetComponent<GameManager>().job == 1)
        {
            if (Input.GetMouseButton(1))
            {
                //camera.transform.position = ADSpos.transform.position;
                ADS = true;
            }
            else
            {
                ADS = false;
            }
            if (Input.GetMouseButtonUp(1))
            {
                //camera.transform.position = NOADSpos.transform.position;
                
            }
            if (ADS == true)
            {
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, ADSpos.transform.position, 10 * Time.deltaTime);

            }
            //else if (ADS == false)
            //{
            //    camera.transform.position = Vector3.MoveTowards(camera.transform.position, NOADSpos.transform.position, 10 * Time.deltaTime);
            //}
           
        }
    }
    void Archercontrol()
    {
        if (Input.GetMouseButton(0) && mouseleft == false)
        {
            if (mouseleft == false && mouseright == false) { GetComponent<Archer＿skill>().NormalAttack(); }
            mouseleft = true;

        }

        if (Input.GetMouseButton(1) && mouseright == false)
        {
            mouseright = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mouseleft == true && mouseright == true) { GetComponent<Archer＿skill>().NormalAttack(); }
            mouseleft = false;

        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseright = false;
            GetComponent<Archer＿skill>().shootpower = 0;
        }

        if (mouseright == true && mouseleft == true)
        {
            if (GetComponent<Archer＿skill>().shootpower <= 30)
            {
                GetComponent<Archer＿skill>().shootpower += 10 * Time.deltaTime;
            }
        }

        if (mouseleft == false)
        {

            a -= Time.deltaTime;
            if (a <= 0)
            {
                GetComponent<Archer＿skill>().shootpower = 0;
            }
        }
        else
        {
            a = 0.5f;
        }
    }

    void LateUpdate()
    {
        // プレイヤーの位置に高さオフセットを加える
        Vector3 basePos = transform.position + Vector3.up * cameraHeight;

        // 後方へ Ray を飛ばす
        Vector3 desiredPos = basePos - transform.forward * defaultDistance;
        Ray ray = new Ray(basePos, -transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * defaultDistance, Color.red);

        if (Physics.Raycast(ray, out hit, defaultDistance))
        {
            float hitDistance = Vector3.Distance(basePos, hit.point);
            Vector3 correctedPos = basePos - transform.forward * (hitDistance - 0.2f);
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, correctedPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        }
    }


}
