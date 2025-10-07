using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    [SerializeField] private float destructionTime;
    [SerializeField] private float throwingPower;
    [SerializeField] public int stonePower;
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwingPower, ForceMode.Impulse);
        Destroy(gameObject, destructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            Destroy(gameObject);
        }
    }
}
