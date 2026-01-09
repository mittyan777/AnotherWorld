using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManerger : MonoBehaviour
{
    public int HP=1000;
    public int AP = 10;

    public void testDamage(int damage) 
    {
        HP-=damage;
        Debug.Log(damage);
        Debug.Log(HP);
    }
}
