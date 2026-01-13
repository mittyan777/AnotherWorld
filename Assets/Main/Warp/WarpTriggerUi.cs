using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTriggerUi : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(false);
        }
    }

}
