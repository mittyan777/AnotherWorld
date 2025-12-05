using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager_hikido : MonoBehaviour
{
    [SerializeField] private GameObject []Player;
    [SerializeField]public float Coin;
    [SerializeField]public float HP;
    [SerializeField]public float MP;
    [SerializeField]public float AttackStatus;
    [SerializeField]public float DefenseStatus;
   
    [SerializeField] float Present_HP;
    [SerializeField]public float Present_MP;
    [SerializeField] TextMeshProUGUI MP_text;
    [SerializeField] TextMeshProUGUI HP_text;


    [SerializeField] public float job;
    [SerializeField] public bool FirstRoulette = true;
    [SerializeField] Text job_text;

    [SerializeField] public int []Status;

    [SerializeField] GameObject rouletteUI;
    [SerializeField] GameObject Weapon_UI;
    [SerializeField] GameObject GAMEUI;
    [SerializeField] GameObject jobroulette;
    [SerializeField] GameObject Standard_UI;
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject []skill_UI;

    [SerializeField] public Image []gage_image;

    [SerializeField] Slider HP_slider;
    [SerializeField] Slider MP_slider;

    public bool slot = false;
    bool Initial_HP = false;
    public bool fade = false;
    [SerializeField] Image fade_image;
    public float fade_image_a = 1;



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Canvas);
        
      
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (FirstRoulette == true)
            {
                if (GAMEUI.activeSelf == false && rouletteUI.activeSelf == false)
                {
                    Time.timeScale = 0.5f;
                    GAMEUI.SetActive(true);
                    rouletteUI.SetActive(true);
                    Standard_UI.SetActive(false);
                }
                else if(GAMEUI.activeSelf == true && rouletteUI.activeSelf == true)
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
        if (FirstRoulette == false)
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
            Player[0].GetComponent<Player_hikido1>().enabled = false;
        }
        else
        {
            Player[0].GetComponent<Player_hikido1>().enabled = true;
        }
        if(job == 0) job_text.text = ($"剣士");
        if (job == 1) job_text.text = ($"アーチャー");
        if (job == 2) 
        {
            job_text.text = ($"魔法使い");
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

        HP_slider.maxValue = HP;
        MP_slider.maxValue = MP;
        HP_slider.value = Present_HP;
        MP_slider.value = Present_MP;

        if(fade == false && fade_image_a >= 0)
        {
            fade_image_a -= Time.deltaTime;
        }
        else if(fade_image_a <= 1)
        {
            fade_image_a += Time.deltaTime;
        }
        fade_image.color = new Color(0,0,0, fade_image_a);
    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }
       
     
       
        Player[0].GetComponent<Player_hikido1>().HP = HP;

       
        Player[0].GetComponent<Player_hikido1>().MP = MP;

      
        Player[0].GetComponent<Player_hikido1>().AttackStatus = AttackStatus;

     
        Player[0].GetComponent<Player_hikido1>().DefenseStatus = DefenseStatus;
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

}
