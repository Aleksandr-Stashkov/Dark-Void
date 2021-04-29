using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private UI_Manager _UI_Manager;    
    private bool _isCoop = false;    
    private bool _canRestart = false; //Restart by R button on death
    
    public static bool isPaused = false;

    private void Start()
    {
        IdentifyScene();
        
        isPaused = false;
        Time.timeScale = 1;

        Find_UI_Manager();
    }

    private void IdentifyScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Substring(0, 4) == "Coop")
        {
            _isCoop = true;
        }
        Debug.Log("Loaded " + sceneName + ".");
    }

    private void Find_UI_Manager()
    {
        _UI_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_UI_Manager == null)
        {
            Debug.LogError("Game Manager could not find Canvas.");
        }
        else
        {
            _UI_Manager.SetGameManager(this);
        }
    }
        
    private void Update()
    {
        if (_canRestart && Input.GetKeyUp(KeyCode.R) && !isPaused)
        {
            ReloadScene();
        }
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause))
        {
            _UI_Manager.Pause(isPaused);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void AllowRestart()
    {
        _canRestart = true;
    }

    public bool IsCoop()
    {
        return _isCoop;
    }
}