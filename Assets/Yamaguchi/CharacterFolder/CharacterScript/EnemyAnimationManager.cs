using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator animator = null;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GolemIdol() 
    {
        animator.SetBool("Idol",true);
        animator.SetBool("Walk", false);
    }
}
