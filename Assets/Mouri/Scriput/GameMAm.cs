using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMAm : MonoBehaviour
{
    [SerializeField] private GameObject[] Player;
    [SerializeField] private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] float AttackStatus;
    [SerializeField] float DefenseStatus;
    [SerializeField] public float job;

    [SerializeField] public int[] Status;
    public bool slot = true;
    [SerializeField] Text powerslot_text;
    [SerializeField] Text defense_text;
    [SerializeField] Text HP_text;
    [SerializeField] Text MP_text;
    [SerializeField] Text job_text;

    [SerializeField] int HP_slot_Max;
    [SerializeField] int HP_slot_Min;

    [SerializeField] int MP_slot_Max;
    [SerializeField] int MP_slot_Min;

    [SerializeField] int power_slot_Max;
    [SerializeField] int power_slot_Min;

    [SerializeField] int Defense_slot_Max;
    [SerializeField] int Defense_slot_Min;
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
            Status[0] = Random.Range(HP_slot_Min, HP_slot_Max);
            Status[1] = Random.Range(MP_slot_Min, MP_slot_Max);
            Status[2] = Random.Range(power_slot_Min, power_slot_Max);
            Status[3] = Random.Range(Defense_slot_Min, Defense_slot_Max);
            Status[4] = Random.Range(1, 4);
        }

        HP_text.text = ($"{Status[0]}");
        MP_text.text = ($"{Status[1]}");
        powerslot_text.text = ($"{Status[2]}");
        defense_text.text = ($"{Status[3]}");
        if (Status[4] == 1) { job_text.text = ($"剣士"); }
        if (Status[4] == 2) { job_text.text = ($"アーチャー"); }
        if (Status[4] == 3) { job_text.text = ($"マジシャン "); }



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
    public void stopslot()
    {
        slot = false;
        HP += Status[0];
        MP += Status[1];
        AttackStatus += Status[2];
        DefenseStatus += Status[3];

    }
}
