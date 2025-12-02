using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherQskill_hikido : PlayerAtackBase
{
    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject[] shootposition;
    public float shootpower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootpower >= 0 && shootpower <= 14)
        {
            Debug.Log("‘æˆê");
        }
        else if (shootpower >= 15 && shootpower <= 30)
        {
            Debug.Log("‘æ2");
        }
        else if (shootpower > 30)
        {
            Debug.Log("‘æ3");
        }
    }
    public void NormalAttack()
    {
        if (shootpower >= 0 && shootpower <= 14)
        {
           
            Instantiate(Arrow, shootposition[0].transform.position, Quaternion.identity);
        }
        else if (shootpower >= 15 && shootpower <= 30)
        {
            
            Instantiate(Arrow, shootposition[0].transform.position, Quaternion.identity);
            Instantiate(Arrow, shootposition[1].transform.position, Quaternion.identity);
        }
        else if (shootpower > 30)
        {
           
            Instantiate(Arrow, shootposition[0].transform.position, Quaternion.identity);
            Instantiate(Arrow, shootposition[1].transform.position, Quaternion.identity);
            Instantiate(Arrow, shootposition[2].transform.position, Quaternion.identity);
        }
    

    }
   
}
