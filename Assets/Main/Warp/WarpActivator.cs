using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpActivator : MonoBehaviour
{
    public GameObject _taregetBoss;        //ŠÄ‹‚·‚é“G
    public GameObject warpObject;               //oŒ»‚³‚¹‚éƒ[ƒv
    public float delay = 2f;                    //“G€–SŒã‚Ì’x‰„ŠÔ

    private bool activated = false;

    private void Start()
    {
        activated = false;
    }

    void Update()
    {
        if (!activated && _taregetBoss == null)
        {
            activated = true;
            StartCoroutine(ActivateWarp());
        }
    }

    IEnumerator ActivateWarp()
    {
        yield return new WaitForSeconds(delay);
        warpObject.SetActive(true);
    }

}
