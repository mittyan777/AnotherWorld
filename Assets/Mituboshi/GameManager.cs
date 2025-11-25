using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject []Player;
    [SerializeField]public float Coin;
    [SerializeField]public float HP;
    [SerializeField]public float MP;
    [SerializeField]public float AttackStatus;
    [SerializeField]public float DefenseStatus;
   
    [SerializeField] float Present_HP;
    [SerializeField] float Present_MP;


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

    [SerializeField] Slider HP_slider;
    [SerializeField] Slider MP_slider;

    public bool slot = false;
    bool Initial_HP = false;




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
                    GAMEUI.SetActive(true);
                    rouletteUI.SetActive(true);
                    Standard_UI.SetActive(false);
                }
                else if(GAMEUI.activeSelf == true && rouletteUI.activeSelf == true)
                {
                    GAMEUI.SetActive(false);
                    rouletteUI.SetActive(false);
                    Standard_UI.SetActive(true);
                }
                else if(GAMEUI.activeSelf == true && Weapon_UI == true)
                {
                    GAMEUI.SetActive(false);
                    Weapon_UI.SetActive(false);
                    Standard_UI.SetActive(true);
                }



            }
            else
            {
                if (GAMEUI.activeSelf == false && rouletteUI.activeSelf == false)
                {
                    GAMEUI.SetActive(true);
                    rouletteUI.SetActive(true);
                    Standard_UI.SetActive(false);
                }
                else if (GAMEUI.activeSelf == true && rouletteUI.activeSelf == true)
                {
                    GAMEUI.SetActive(false);
                    rouletteUI.SetActive(false);
                    Standard_UI.SetActive(true);
                }
                else if (GAMEUI.activeSelf == true && Weapon_UI == true)
                {
                    GAMEUI.SetActive(false);
                    Weapon_UI.SetActive(false);
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


        if (rouletteUI.activeSelf == true || Weapon_UI.activeSelf == true)
        {
            Player[0].GetComponent<Player>().enabled = false;
        }
        else
        {
            Player[0].GetComponent<Player>().enabled = true;
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
        HP_slider.maxValue = HP;
        MP_slider.maxValue = MP;
        HP_slider.value = Present_HP;
        MP_slider.value = Present_MP;


    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }
       
     
       
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

}
