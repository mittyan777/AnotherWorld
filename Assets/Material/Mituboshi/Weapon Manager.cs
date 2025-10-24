using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject []Player;
    [SerializeField] GameObject  Hand;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject stick;
    [SerializeField] GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null) { Player = GameObject.FindGameObjectsWithTag("Player"); }
        if (manager.GetComponent<GameManager>().Status[4] == 1 && manager.GetComponent<GameManager>().slot == false)
        {
            sword.gameObject.transform.parent = Hand.gameObject.transform;
            sword.transform.position = Hand.transform.position;
        }
        if (manager.GetComponent<GameManager>().Status[4] == 4 && manager.GetComponent<GameManager>().slot == false)
        {
            stick.gameObject.transform.parent = Hand.gameObject.transform;
            stick.transform.position = Hand.transform.position;
             
        }
    }
}
