using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player_hikido1 : MonoBehaviour
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

    //Ç–Ç´Ç«í«â¡
    [SerializeField] AnimationFlagManagerSO _animflgSO;
    [SerializeField] PlayerAnimation playerAnim;

    [SerializeField] float a = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0f;
        animator = GetComponent<Animator>();
        // Cursor.lockState = CursorLockMode.Locked;   //í«â¡
        //Cursor.visible = false;     //í«â¡
    }

    // Update is called once per frame
    void Update()
    {
        if (manager == null) { manager = GameObject.FindGameObjectWithTag("GameManager"); }
        cameracontrol();

        rayobj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        if (!canControl) return; // Ç±Ç±Ç≈ëÄçÏëSïîé~Ç‹ÇÈ

        // ëOï˚Ç…RayÇîÚÇŒÇµÇƒÅAÉãÅ[ÉåÉbÉgë‰Ç…ìñÇΩÇ¡ÇΩÇÁ
        Ray ray = new Ray(new Vector3(rayobj.transform.position.x, rayobj.transform.position.y, rayobj.transform.position.z), rayobj.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * Direction, Color.red);
        if (UnityEngine.Input.GetKeyDown("p"))
        {
            manager.GetComponent<GameManager_hikido>().HP -= 10;
        }
        if (manager.GetComponent<GameManager_hikido>().HP <= 0)
        {
            //Destroy(gameObject);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) && manager.GetComponent<GameManager_hikido>().Present_MP >= 10 && manager.GetComponent<GameManager_hikido>().gage_image[1].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager_hikido>().job == 2)
            {
                manager.GetComponent<GameManager_hikido>().gage_image[1].fillAmount = 0;
                GetComponent<Magician_Skills_hikido>().FireBall();
                manager.GetComponent<GameManager_hikido>().Present_MP -= 10;

                playerAnim.SetWizardSkillIndex(1);
                _animflgSO.MajicSkilFlg = true;
            }


        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) && manager.GetComponent<GameManager_hikido>().Present_MP >= 10 && manager.GetComponent<GameManager_hikido>().gage_image[0].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager_hikido>().job == 2)
            {
                manager.GetComponent<GameManager_hikido>().gage_image[0].fillAmount = 0;
                GetComponent<Magician_Skills_hikido>().ElectricBall();
                manager.GetComponent<GameManager_hikido>().Present_MP -= 10;

                playerAnim.SetWizardSkillIndex(2);
                _animflgSO.MajicSkilFlg = true;
            }


        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) && manager.GetComponent<GameManager_hikido>().Present_MP >= 10 && manager.GetComponent<GameManager_hikido>().gage_image[2].fillAmount >= 1)
        {
            if (manager.GetComponent<GameManager_hikido>().job == 2)
            {
                manager.GetComponent<GameManager_hikido>().gage_image[2].fillAmount = 0;
                _animflgSO.MajicSkilFlg = true;
                GetComponent<Magician_Skills_hikido>().TornadoAttack();
                manager.GetComponent<GameManager_hikido>().Present_MP -= 10;

                playerAnim.SetWizardSkillIndex(3);
                _animflgSO.MajicSkilFlg = true;
            }

        }

        
        if(manager.GetComponent<GameManager_hikido>().job == 2) 
        {
            bool normalAtkmajic = UnityEngine.Input.GetMouseButtonDown(0);
            if(normalAtkmajic)
            { _animflgSO.MajicAttackNormalFlg = true; }
            else 
            { _animflgSO.MajicAttackNormalFlg = false; }
           
        }


        if (manager.GetComponent<GameManager_hikido>().job == 1)
        {
            Archercontrol();
        }

        if(manager.GetComponent<GameManager_hikido>().job == 0) 
        {
            SwordSkil();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            manager.GetComponent<GameManager_hikido>().Coin += 100;
            Destroy(other.gameObject);
        }
    }
    void cameracontrol()
    {
        float mx = UnityEngine.Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float my = UnityEngine.Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mx;
        xRotation -= my;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (manager.GetComponent<GameManager_hikido>().job == 1)
        {
            if (UnityEngine.Input.GetMouseButton(1))
            {
                //camera.transform.position = ADSpos.transform.position;
                ADS = true;
            }
            else
            {
                ADS = false;
            }
            if (UnityEngine.Input.GetMouseButtonUp(1))
            {
                //camera.transform.position = NOADSpos.transform.position;

            }
            if (ADS == true)
            {
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, ADSpos.transform.position, 10 * Time.deltaTime);

            }
            else if (ADS == false)
            {
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, NOADSpos.transform.position, 10 * Time.deltaTime);
            }

        }
    }
    void Archercontrol()
    {
        if (UnityEngine.Input.GetMouseButton(0) && mouseleft == false)
        {
            if (mouseleft == false && mouseright == false) 
            {
                GetComponent<ArcherÅQskill_hikido>().NormalAttack();
            }
            mouseleft = true;
            _animflgSO.ArcherSkilflg = true;

        }

        if (UnityEngine.Input.GetMouseButton(1) && mouseright == false)
        {
            mouseright = true;

        }

        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            if (mouseleft == true && mouseright == true) 
            {
                GetComponent<ArcherÅQskill_hikido>().NormalAttack(); 
            }
            mouseleft = false;

            _animflgSO.ArcherSkilflg = false;
        }

        if (UnityEngine.Input.GetMouseButtonUp(1))
        {
            mouseright = false;
            GetComponent<ArcherÅQskill_hikido>().shootpower = 0;
        }

        if (mouseright == true && mouseleft == true)
        {
            if (GetComponent<ArcherÅQskill_hikido>().shootpower <= 30)
            {
                GetComponent<ArcherÅQskill_hikido>().shootpower += 10 * Time.deltaTime;
            }
        }

        if (mouseleft == false)
        {

            a -= Time.deltaTime;
            if (a <= 0)
            {
                GetComponent<ArcherÅQskill_hikido>().shootpower = 0;
            }
        }
        else
        {
            a = 0.5f;
        }
    }

    //Ç–Ç´Ç«í«â¡ï™
    void SwordSkil() 
    {
        if (UnityEngine.Input.GetMouseButtonDown(0)) 
        {
            GetComponent<Player_Swoad>().SwordSkill();
        }
    }

    //Ç–Ç´Ç«í«â¡ï™
    public bool isAiming 
    {
        get { return ADS; }
    }

    //ìØÇ∂Ç≠Ç–Ç´Ç«í«â¡ï™
    public void HandleStanderdMovement(Transform transform, Animator animator) 
    {
        if (UnityEngine.Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            animator.SetBool("walk", true);
        }
        else { animator.SetBool("walk", false); }
        if (UnityEngine.Input.GetKey("s"))
        {
            transform.position -= transform.forward * 5 * Time.deltaTime;
            animator.SetBool("back", true);
        }
        else { animator.SetBool("back", false); }
        if (UnityEngine.Input.GetKey("a"))
        {
            transform.position -= transform.right * 5 * Time.deltaTime;
            animator.SetBool("left", true);
        }
        else { animator.SetBool("left", false); }
        if (UnityEngine.Input.GetKey("d"))
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            animator.SetBool("right", true);
        }
        else { animator.SetBool("right", false); }
    }

}
