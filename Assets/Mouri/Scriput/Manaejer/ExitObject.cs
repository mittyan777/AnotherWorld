using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitObject : MonoBehaviour
{
    [SerializeField] private DorScripu targetDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetDoor.LockDoor();

        }

    }

}
