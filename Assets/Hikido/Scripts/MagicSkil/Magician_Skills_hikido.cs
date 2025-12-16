using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_Skills_hikido : PlayerAtackBase
{
    [SerializeField] GameObject shootposition;
    [SerializeField] GameObject TornadoParticleposition;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject Electric_ball;
    [SerializeField] GameObject Tornado;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FireBall()
    {
        Instantiate(fireball, shootposition.transform.position, Quaternion.identity);
    }
    public void ElectricBall()
    {
        Instantiate(Electric_ball, shootposition.transform.position, Quaternion.identity);
    }
    public void TornadoAttack()
    {
        Instantiate(Tornado, TornadoParticleposition.transform.position, Quaternion.identity);
    }

}
