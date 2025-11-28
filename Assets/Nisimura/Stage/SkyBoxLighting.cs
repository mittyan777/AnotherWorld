using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxLighting : MonoBehaviour
{
    //”’l‚ª¬‚³‚¢‚Ælight‚Ì‰ñ“]‘¬“x‚ª’x‚­‚È‚é
    [SerializeField] float time;
    private void Update()
    {
        transform.Rotate(new Vector3(0,-time)*Time.deltaTime);
    }
}
