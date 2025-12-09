using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashingMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float breakTime;
    //[SerializeField] private GameObject slashObj;
    //[SerializeField] private float slashTime;
    private void Start()
    {
        StartCoroutine(Break());
    }
    private void Update()
    {
        //StartCoroutine(Slash());
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    /*private IEnumerator Slash() 
    { 
        yield return new WaitForSeconds(slashTime);
        Instantiate(slashObj,transform.position,Quaternion.identity);
    }
    */
    private IEnumerator Break() 
    { 
        yield return new WaitForSeconds(breakTime);
        Destroy(gameObject);
    }
}
