using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricaldomain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("des", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void des()
    {
        Destroy(gameObject);
    }
}
