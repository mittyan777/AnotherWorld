using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class GameManger_Mouri : MonoBehaviour
{
    [SerializeField] private GameObject[] Player;
    [SerializeField] private float Coin;
    [SerializeField] float HP;
    [SerializeField] float MP;
    [SerializeField] float AttackStatus;
    [SerializeField] float DefenseStatus;

    [SerializeField] public int[] Status;
    public bool slot = true;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus();


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
}
