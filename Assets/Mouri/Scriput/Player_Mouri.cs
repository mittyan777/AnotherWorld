using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Mouri: MonoBehaviour
{
    
    Animator animator;

    public float HP;
    public float MP;
    public float AttackStatus;
    public float DefenseStatus;
    public bool canControl = true;  // Å© í«â¡ÅI
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
    [SerializeField] bool ADS = false;

    [SerializeField] float a = 0.5f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float defaultDistance = 5f; // í èÌÇÃÉJÉÅÉâãóó£
    [SerializeField] private float smoothSpeed = 10f;    // ï‚ä‘ë¨ìx
    [SerializeField] private float cameraHeight = 1.5f;

    [SerializeField] private AudioSource WalkSource;
    [SerializeField] private AudioClip WalkClip;


    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0f;
        animator = GetComponent<Animator>();
        // Cursor.lockState = CursorLockMode.Locked;   //í«â¡
        //Cursor.visible = false;     //í«â¡

        WalkSource.clip = WalkClip;
        WalkSource.loop = true;

    }

    bool isMoving=false;

    // Update is called once per frame
    void Update()
    {
        if (manager == null) { manager = GameObject.FindGameObjectWithTag("GameManager"); }
        cameracontrol();



        rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // Ç±Ç±Ç≈ëÄçÏëSïîé~Ç‹ÇÈ


        if (Input.GetKeyDown("p"))
        {
            manager.GetComponent<GameManager>().HP -= 10;
        }
        if (manager.GetComponent<GameManager>().HP <= 0)
        {
            //Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[1].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2)
            {
                manager.GetComponent<GameManager>().gage_image[1].fillAmount = 0;
                GetComponent<Magician_Skills>().FireBall();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[0].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2)
            {
                manager.GetComponent<GameManager>().gage_image[0].fillAmount = 0;
                GetComponent<Magician_Skills>().ElectricBall();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }


        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && manager.GetComponent<GameManager>().Present_MP >= 10 && manager.GetComponent<GameManager>().gage_image[2].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager>().job == 2)
            {
                manager.GetComponent<GameManager>().gage_image[2].fillAmount = 0;
                GetComponent<Magician_Skills>().TornadoAttack();
                manager.GetComponent<GameManager>().Present_MP -= 10;
            }

        }

        if (manager.GetComponent<GameManager>().job == 1)
        {
            Archercontrol();

        }

    }
    private void FixedUpdate()
    {
        isMoving = false;
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            animator.SetBool("walk", true);
            isMoving = true;
        }
        else { animator.SetBool("walk", false); }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * 5 * Time.deltaTime;
            animator.SetBool("back", true);
            isMoving = true;
        }
        else { animator.SetBool("back", false); }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * 5 * Time.deltaTime;
            animator.SetBool("left", true);
            isMoving = true;
        }
        else { animator.SetBool("left", false); }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            animator.SetBool("right", true);
            isMoving = true;
        }

        else { animator.SetBool("right", false); }

        if (isMoving&&!WalkSource.isPlaying)WalkSource.Play();   //í«â¡

        else if(!isMoving&&WalkSource.isPlaying)WalkSource.Stop();
        



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
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
            if (mouseleft == false && mouseright == false) { GetComponent<ArcherÅQskill>().NormalAttack(); }
            mouseleft = true;

        }

        if (Input.GetMouseButton(1) && mouseright == false)
        {
            mouseright = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mouseleft == true && mouseright == true) { GetComponent<ArcherÅQskill>().NormalAttack(); }
            mouseleft = false;

        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseright = false;
            GetComponent<ArcherÅQskill>().shootpower = 0;
        }

        if (mouseright == true && mouseleft == true)
        {
            if (GetComponent<ArcherÅQskill>().shootpower <= 30)
            {
                GetComponent<ArcherÅQskill>().shootpower += 10 * Time.deltaTime;
            }
        }

        if (mouseleft == false)
        {

            a -= Time.deltaTime;
            if (a <= 0)
            {
                GetComponent<ArcherÅQskill>().shootpower = 0;
            }
        }
        else
        {
            a = 0.5f;
        }
    }

    void LateUpdate()
    {
        // ÉvÉåÉCÉÑÅ[ÇÃà íuÇ…çÇÇ≥ÉIÉtÉZÉbÉgÇâ¡Ç¶ÇÈ
        Vector3 basePos = transform.position + Vector3.up * cameraHeight;

        // å„ï˚Ç÷ Ray ÇîÚÇŒÇ∑
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
