using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float throwingPower;
    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwingPower, ForceMode.Impulse);
    }
}
