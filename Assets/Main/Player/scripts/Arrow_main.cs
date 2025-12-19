using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_main : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject player;
    [SerializeField]float power;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        power = player.GetComponent<ArcherQskill_main>().shootpower;
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = player.transform.eulerAngles;
        if (power >= 0 && power <= 14)
        {
            rb.velocity = player.transform.forward * 10;
        }
        else if(power >= 15 && power <= 30)
        {
            rb.velocity = player.transform.forward * 20;
        }
        else if (power > 30)
        {
            rb.velocity = player.transform.forward * 30;
        }
       
    
        Invoke("des",10);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void des ()
    {
        Destroy(gameObject);
    }
}
