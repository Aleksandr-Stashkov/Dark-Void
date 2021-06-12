using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PauseMenu _pauseMenu;    
    private bool _isCoop = false;    
    private bool _canRestart = false; //Restart by R button on death
    
    public static bool isPaused = false;

    private void Start()
    {
        IdentifyScene();
        
        isPaused = false;
        Time.timeScale = 1;

        FindObjects();
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

    private void FindObjects()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Game Manager could not find Canvas.");
        }
        else
        {
            UI_Manager manager = canvas.GetComponent<UI_Manager>();
            if (manager == null)
            {
                Debug.LogError("Game Manager could not find UI Manager on Canvas.");
            }
            else
            {
                manager.SetGameManager(this);
            }

            _pauseMenu = canvas.GetComponent<PauseMenu>();
            if (_pauseMenu == null)
            {
                Debug.LogError("Game Manager could not find Pause Menu script.");
            }
            else
            {
                _pauseMenu.SetGameManager(this);
            }
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
            _pauseMenu.Pause(isPaused);
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