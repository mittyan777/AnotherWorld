using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpActivator : MonoBehaviour
{
    public BossMoveManager _taregetBoss;        //ŠÄ‹‚·‚é“G
    public GameObject warpObject;               //oŒ»‚³‚¹‚éƒ[ƒv
    public float delay = 2f;                    //“G€–SŒã‚Ì’x‰„ŠÔ

    private bool activated = false;

    void Update()
    {
        if (!activated && _taregetBoss.currentBossHP <= 0)
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
