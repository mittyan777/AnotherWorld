using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_load_trigger : MonoBehaviour
{
    [SerializeField] GameObject GameManager;
    [SerializeField] GameObject Player;
    bool start_trigger = true;
    [SerializeField] int Distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if(distance <= Distance)
        {
            GameManager.GetComponent<GameManager>().fade = true;
            start_trigger = false;
        }

        if (GameManager.GetComponent<GameManager>().fade_image_a >= 1 && start_trigger == false)
        {
            SceneManager.LoadScene("load_screen");
            start_trigger = true;
        }
    }
}
