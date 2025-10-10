using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangWindow : MonoBehaviour
{
    public void change_button()
    {
        SceneManager.LoadScene("New Scene");
    }
}
