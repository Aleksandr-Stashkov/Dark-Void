using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{   
    public void Start_Button()
    {
        SceneManager.LoadScene(1);
    }

    public void Coop_Button()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit_Button()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Exit_Button();
        }
    }
}
