using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    [SerializeField] private float throwingPower;
    [SerializeField] private float destroyTime;
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwingPower, ForceMode.Impulse);
        Destroy(gameObject, destroyTime);
    }
}
