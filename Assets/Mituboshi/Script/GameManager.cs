using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject []Player;
    [SerializeField]public float Coin;
    [SerializeField]public float HP;
    [SerializeField]public float MP;
    [SerializeField]public float AttackStatus;
    [SerializeField]public float DefenseStatus;
   
    [SerializeField] public float Present_HP;
    [SerializeField]public float Present_MP;
    [SerializeField] TextMeshProUGUI MP_text;
    [SerializeField] TextMeshProUGUI HP_text;


    [SerializeField] public float job;
    [SerializeField] Image job_text;
    [SerializeField] Sprite []job_sprite;

    [SerializeField] public int []Status;

    [SerializeField] GameObject rouletteUI;
    [SerializeField] GameObject Weapon_UI;
    [SerializeField] GameObject GAMEUI;
    [SerializeField] GameObject jobroulette;
    [SerializeField] GameObject Standard_UI;
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject []skill_UI;
    [SerializeField] GameObject UI_change_button;

    [SerializeField] public Image []gage_image;

    [SerializeField] Image HP_slider;
    [SerializeField] Image MP_slider;

    public bool slot = false;
    bool Initial_HP = false;
    public bool fade = false;
    [SerializeField] Image fade_image;
    public float fade_image_a = 1;

    public string scene_name;
    public int tutorial_count = 0;
    [SerializeField]GameObject tutorial_UI;
     float speed = 50;      // ˆÚ“®‘¬“x
     float distance = 10;   // ˆÚ“®‹——£

    private Vector3 startPos;
    [SerializeField] GameObject[] Status_text;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Canvas);
      
    }
    

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "load_screen" && SceneManager.GetActiveScene().name != "start")
        {
            Canvas.SetActive(true);
            if (Player[0] == null)
            {
                Player = GameObject.FindGameObjectsWithTag("Player");
            }
           // PlayerStatus();
        }
        else { Canvas.SetActive(false); }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.Find("Rouret_Manejer").GetComponent<Rouretto_New>().FirstRoulette == true)
            {
                
                if (GAMEUI.activeSelf == true && rouletteUI.activeSelf == true)
                {
                    Time.timeScale = 1;
                    GAMEUI.SetActive(false);
                    rouletteUI.SetActive(false);
                    Standard_UI.SetActive(true);
                    

                }
                else if(GAMEUI.activeSelf == true && Weapon_UI == true)
                {
                    Time.timeScale = 1;
                    GAMEUI.SetActive(false);
                    Weapon_UI.SetActive(false);
                    Standard_UI.SetActive(true);
                    
                }



            }
            else
            {
                if (GAMEUI.activeSelf == false && rouletteUI.activeSelf == false)
                {
                    Time.timeScale = 0.5f;
                    GAMEUI.SetActive(true);
                    rouletteUI.SetActive(true);
                    jobroulette.SetActive(false);
                    Standard_UI.SetActive(false);
                    Destroy(tutorial_UI.gameObject);
                }
                else if (GAMEUI.activeSelf == true && rouletteUI.activeSelf == true)
                {
                    Time.timeScale = 1f;
                    GAMEUI.SetActive(false);
                    rouletteUI.SetActive(false);
                    jobroulette.SetActive(false);
                    Standard_UI.SetActive(true);
                }
                else if (GAMEUI.activeSelf == true && Weapon_UI == true)
                {
                    Time.timeScale = 1;
                    GAMEUI.SetActive(false);
                    Weapon_UI.SetActive(false);
                    jobroulette.SetActive(false);
                    Standard_UI.SetActive(true);
                }
            }
          
        }
        if (GameObject.Find("Rouret_Manejer").GetComponent<Rouretto_New>().FirstRoulette == true)
        {
            if (SceneManager.GetActiveScene().name == "EnemyScene_mitubosi")
            {
                if (GAMEUI.activeSelf == false && rouletteUI.activeSelf == false)
                {
                    Time.timeScale = 0.5f;
                    GAMEUI.SetActive(true);
                    rouletteUI.SetActive(true);
                    Standard_UI.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Rouret_Manejer").GetComponent<Rouretto_New>().FirstRoulette == false)
        {
            if(Initial_HP == false) 
            { 
                Present_HP = HP;
                Present_MP = MP;
                Initial_HP = true;
            }

        }

       

        for (int i = 0; i < gage_image.Length; i++)
        {
            if (gage_image[i].fillAmount < 1)
            {
                gage_image[i].fillAmount += 0.5f * Time.deltaTime;
            }
        }


        if (rouletteUI.activeSelf == true || Weapon_UI.activeSelf == true)
        {
            Player[0].GetComponent<Player>().enabled = false;
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "load_screen" && SceneManager.GetActiveScene().name != "start") { Player[0].GetComponent<Player>().enabled = true; }
        }
        if (job == 0) 
        {
            job_text.sprite = job_sprite[0];
        }
        if (job == 1) 
        {
            job_text.sprite = job_sprite[1];
            
        }
        if (job == 2) 
        {
            job_text.sprite = job_sprite[2];
            for (int i = 0; i < 3; i++)
            {
                skill_UI[i].SetActive(true);
            }        
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                skill_UI[i].SetActive(false);
            }
        }

        if(Present_HP <= 0)
        {
            fade = true;
        }
      

        HP_text.text = ($"{Mathf.Ceil(Present_HP)} / {Mathf.Ceil(HP)} ");
        MP_text.text = ($"{Mathf.Ceil(Present_MP)} / {Mathf.Ceil(MP)} ");

        if (Present_MP < MP) { Present_MP += 2 * Time.deltaTime; }
        if (Present_MP >= MP) { Present_MP = MP; }

       
        HP_slider.fillAmount = Present_HP / HP;
        MP_slider.fillAmount = Present_MP / MP;

        if(fade == false && fade_image_a >= 0)
        {
            fade_image_a -= Time.deltaTime;
        }
        else if(fade_image_a <= 1)
        {
            fade_image_a += Time.deltaTime;
        }
        fade_image.color = new Color(0,0,0, fade_image_a);

        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("load_screen");
        }

        if (SceneManager.GetActiveScene().name == "Synthesis_Test")
        {
            scene_name = "EnemyScene_mitubosi";
        }
        if (tutorial_UI != null)
        {
            if (tutorial_count == 0)
            {
                for (int i = 0; i < Status_text.Length; i++)
                {
                    Status_text[i].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                startPos = new Vector3(-17, 106, -2);
                float offset = Mathf.PingPong(Time.time * speed, distance);
                tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(0, offset, 0);
                tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -238);
                tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                UI_change_button.SetActive(false);
            }
            else if (tutorial_count == 1)
            {
                for (int i = 0; i < Status_text.Length; i++)
                {
                    Status_text[i].transform.Rotate(3, 0, 0);
                }
                startPos = new Vector3(344, 106, -2);
                float offset = Mathf.PingPong(Time.time * speed, distance);
                tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(0, offset, 0);
                //tutorial_UI.GetComponent<RectTransform>().localPosition = new Vector3(344, 106, -2);
                tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -306);
                tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            }
            else if (tutorial_count == 2)
            {
                for (int i = 0; i < Status_text.Length; i++)
                {
                    Status_text[i].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                startPos = new Vector3(-165, 335, -2);
                float offset = Mathf.PingPong(Time.time * speed, distance);
                tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(offset, 0, 0);
                //tutorial_UI.GetComponent<RectTransform>().localPosition = new Vector3(-165, 335, -2);
                tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -385);
                tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                UI_change_button.SetActive(true);
            }
            else if (tutorial_count == 3)
            {
                if (job == 0)
                {
                    startPos = new Vector3(145, 100, -2);
                    float offset = Mathf.PingPong(Time.time * speed, distance);
                    tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(offset, 0, 0);
                    //tutorial_UI.GetComponent<RectTransform>().localPosition = new Vector3(185, 100, -2);
                    tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                }
                else if (job == 2)
                {
                    startPos = new Vector3(145, 55, -2);
                    float offset = Mathf.PingPong(Time.time * speed, distance);
                    tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(offset, 0, 0);
                    //tutorial_UI.GetComponent<RectTransform>().localPosition = new Vector3(185, 55, -2);
                    tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                }
                else if (job == 1)
                {
                    startPos = new Vector3(145, 5, -2);
                    float offset = Mathf.PingPong(Time.time * speed, distance);
                    tutorial_UI.GetComponent<RectTransform>().transform.localPosition = startPos + new Vector3(offset, 0, 0);
                    // tutorial_UI.GetComponent<RectTransform>().localPosition = new Vector3(185, 5, -2);
                    tutorial_UI.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    tutorial_UI.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                }
            }
            else
            {
                Destroy(tutorial_UI.gameObject);
            }
        }

    }
    private void PlayerStatus()
    {
       
       
     
       
        Player[0].GetComponent<Player>().HP = HP;

       
        Player[0].GetComponent<Player>().MP = MP;

      
        Player[0].GetComponent<Player>().AttackStatus = AttackStatus;

     
        Player[0].GetComponent<Player>().DefenseStatus = DefenseStatus;
    }
   
    public void UIchange()
    {
        if(rouletteUI.activeSelf == true)
        {
            rouletteUI.SetActive(false);
            Weapon_UI.SetActive(true);
        }
        
    }
    public void UIchange2()
    {
        
        if(rouletteUI.activeSelf == false)
        {
            rouletteUI.SetActive(true);
            Weapon_UI.SetActive(false);
        }
    }
    public void tutorial()
    {
        tutorial_count++;
    }
    public void reset_tutorial()
    {
        if (GameObject.Find("Rouret_Manejer").GetComponent<Rouretto_New>().FirstRoulette == true)
        {
            HP = 0;
            MP = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        Present_HP -= damage;
        Debug.Log(damage);
        Debug.Log(HP);
    }


}
