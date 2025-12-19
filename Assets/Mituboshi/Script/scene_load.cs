using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class scene_load : MonoBehaviour
{
    [SerializeField] Slider _slider;
    AsyncOperation async;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {

        _slider.value += 0.1f * Time.deltaTime;
        if(_slider.value >= 1f)
        {
            SceneManager.LoadScene(gameManager.scene_name);
        }
    }
}
