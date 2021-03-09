using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private bool CoopMode = false;
    private bool Restart = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Substring(0,4) == "Coop")
        {
            CoopMode = true;
        }
        Debug.Log("Loaded " + SceneManager.GetActiveScene().name + ".");
    }
        
    void Update()
    {
        if (Restart && Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Level_01");
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void Restart_act()
    {
        Restart = true;
    }

    public bool Check_Coop()
    {
        return CoopMode;
    }
}
