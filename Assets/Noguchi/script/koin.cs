using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koin : MonoBehaviour
{
    [SerializeField] public int aa;
    public  int playerCoin = 0;
    private void Start()
    {
        playerCoin = aa;
        
    }

}