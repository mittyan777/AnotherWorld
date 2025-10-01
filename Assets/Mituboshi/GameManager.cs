using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject []Player;
    [SerializeField]private float Coin;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       if (Player[0] == null)
       {
            Player = GameObject.FindGameObjectsWithTag("Player");
       }
       Coin = Player[0].GetComponent<Player>().coin;
        
    }
}
