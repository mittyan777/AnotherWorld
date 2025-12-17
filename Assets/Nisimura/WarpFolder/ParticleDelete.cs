using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ワープオブジェクトを消す用のスクリプト
public class ParticleDelete : MonoBehaviour
{

    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particle.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
