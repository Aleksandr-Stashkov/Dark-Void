using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private bool Restart = false;

    void Start()
    {
        
    }
        
    void Update()
    {
        if (Restart && Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Restart_act()
    {
        Restart = true;
    }
}
