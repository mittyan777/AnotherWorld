using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject player;
    [SerializeField]GameObject Thunder_Magic;
    [SerializeField]int des_count = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.velocity = player.transform.forward * 14;
        Invoke("des", 10);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            if (gameObject.name == "Electric_ball(Clone)")
            {
                Instantiate(Thunder_Magic, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            if (gameObject.name == "fireball(Clone)")
            {
              
                
                des_count += 1;
                if(des_count == 4)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    void des()
    {
        Destroy(gameObject );
    }
}

