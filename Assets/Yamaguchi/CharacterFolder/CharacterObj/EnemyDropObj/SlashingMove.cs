using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashingMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float breakTime;
    private void Start()
    {
        StartCoroutine(Break());
    }
    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private IEnumerator Break() 
    { 
        yield return new WaitForSeconds(breakTime);
        Destroy(gameObject);
    }
}
