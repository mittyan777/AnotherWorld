using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackAreaDelete : MonoBehaviour
{
    [SerializeField] private float collisiondelete;
    [SerializeField] private float deleteTime;
    private void Start()
    {
        StartCoroutine(Delete());
    }
    //当たり判定を消してからオブジェクト事態を消すようにした。
    private IEnumerator Delete() 
    { 
        yield return new WaitForSeconds(collisiondelete);
        var collision=GetComponent<Collider>();
        collision.enabled = false;
        yield return new WaitForSeconds(deleteTime);
        Destroy(this.gameObject); 
    }
}
