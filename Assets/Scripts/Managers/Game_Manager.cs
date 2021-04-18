using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private UI_Manager _UI_Manager;
    //General parameters
    private bool CoopMode = false;
    private bool Restart_flag = false; //Restart at death
    public static bool IsPaused = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Substring(0,4) == "Coop")
        {
            CoopMode = true;
        }
        Debug.Log("Loaded " + SceneManager.GetActiveScene().name + ".");
        IsPaused = false;
        Time.timeScale = 1;        
    }
        
    void Update()
    {
        if (Restart_flag && Input.GetKeyUp(KeyCode.R) && !IsPaused)
        {
            Reload();
        }

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause))
        {
            _UI_Manager.Pause(IsPaused);
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Load_Main_Menu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void Restart_act()
    {
        Restart_flag = true;
    }

    public bool Check_Coop()
    {
        return CoopMode;
    }

    //Getting reference to the Pause Menu from UI Manager
    public void Set_UI_Manager(UI_Manager _UI)
    {
        if (_UI != null)
        {
            _UI_Manager = _UI;           
        }
        else
        {
            Debug.LogWarning("Game Manager was handled an empty reference to UI Manager.");
        }
    }    
}
