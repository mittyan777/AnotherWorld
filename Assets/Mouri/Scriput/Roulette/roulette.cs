using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roulette : MonoBehaviour
{
    [SerializeField]int power = 0;
    bool powerslot = true;
    [SerializeField] Text powerslot_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
       
        if (powerslot == true) { power = Random.Range(1, 100); }
       
        powerslot_text.text = ($"{power}");
    }
   public void stopslot()
    {
        powerslot = false;
    }
}
