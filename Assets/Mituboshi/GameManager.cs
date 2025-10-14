using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject []Player;
    [SerializeField]private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] float AttackStatus;
    [SerializeField] float DefenseStatus;

    [SerializeField] int []Status;
    bool slot = true;
    [SerializeField] Text powerslot_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;

    [SerializeField] int slot_Max;
    [SerializeField] int slot_Min;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus();

        if (slot == true) 
        { 
            Status[0] = Random.Range(slot_Min, slot_Max);
            Status[1] = Random.Range(slot_Min, slot_Max);
            Status[2] = Random.Range(slot_Min, slot_Max);
            Status[3] = Random.Range(slot_Min, slot_Max);
        }

        HP_text.text = ($"{Status[0]}");
        MP_text.text = ($"{Status[1]}");
        powerslot_text.text = ($"{Status[2]}");
        defense_text.text = ($"{Status[3]}");

    }
    private void PlayerStatus()
    {
        if (Player[0] == null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }
        Coin = Player[0].GetComponent<Player>().coin;
     
       
        Player[0].GetComponent<Player>().HP = HP;

       
        Player[0].GetComponent<Player>().MP = MP;

      
        Player[0].GetComponent<Player>().AttackStatus = AttackStatus;

     
        Player[0].GetComponent<Player>().DefenseStatus = DefenseStatus;
    }
    public void stopslot()
    {
        slot = false;
        HP += Status[0];
        MP += Status[1];
        AttackStatus += Status[2];
        DefenseStatus += Status[3];

    }
}
