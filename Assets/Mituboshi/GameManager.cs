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
    [SerializeField] public float job;
    [SerializeField] public bool FirstRoulette = true;

    [SerializeField] public int []Status;

    [SerializeField] GameObject rouletteUI;
    [SerializeField] GameObject Weapon_UI;
    [SerializeField] GameObject jobroulette;
    [SerializeField] GameObject Canvas;

    public bool slot = false;

 
   

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
                if (rouletteUI.activeSelf == false && Weapon_UI.activeSelf == false)
                {
                    rouletteUI.SetActive(true);
                }
                else if (Weapon_UI.activeSelf == false)
                {
                    rouletteUI.SetActive(false);

                }
            }
            else
            {
                if (rouletteUI.activeSelf == false && Weapon_UI.activeSelf == false)
                {
                    rouletteUI.SetActive(true);
                    jobroulette.SetActive(false);
                }
                else if (Weapon_UI.activeSelf == false)
                {
                    rouletteUI.SetActive(false);

                }
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
