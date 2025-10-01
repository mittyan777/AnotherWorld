using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;  
    public float coin = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(Input.GetKey("w"))
        {
            transform.position += transform.forward * 5 * Time.deltaTime;
            animator.SetBool("walk", true);
        }
        else { animator.SetBool("walk", false); }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * 5 * Time.deltaTime;
            animator.SetBool("back", true);
        }
        else { animator.SetBool("back", false); }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * 5 * Time.deltaTime;
            animator.SetBool("left", true);
        }
        else { animator.SetBool("left", false); }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            animator.SetBool("right", true);
        }
        else { animator.SetBool("right", false); }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            coin += 1;
            Destroy(other.gameObject);
        }
    }
}
