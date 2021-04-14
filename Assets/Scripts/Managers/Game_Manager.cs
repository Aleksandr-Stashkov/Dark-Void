using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    //Pause Menu components
    private GameObject _Pause_Menu;
    private Animator _anim;
    private int anim_Unpaused_id;
    //General parameters
    private bool CoopMode = false;
    private bool Restart = false;
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
        if (Restart && Input.GetKeyUp(KeyCode.R) && !IsPaused)
        {
            Reload();
        }

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause))
        {
            Pause(IsPaused);
        }
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Restart_act()
    {
        Restart = true;
    }

    public bool Check_Coop()
    {
        return CoopMode;
    }

    //Getting reference to the Pause Menu from UI Manager
    public void Set_Pause_Menu(GameObject _Menu)
    {
        if (_Menu != null)
        {
            _Pause_Menu = _Menu;
            _anim = _Pause_Menu.GetComponent<Animator>();
            if (_anim == null)
            {
                Debug.LogError("Game Manager could not locate Pause Menu's animator.");
            }
            anim_Unpaused_id = Animator.StringToHash("Unpaused");
            if (anim_Unpaused_id == 0)
            {
                Debug.LogWarning("Game Manager could not find Unpaused parameter of the Pause Menu animator.");
            }
        }
        else
        {
            Debug.LogWarning("Game Manager was handled an empty reference to Pause Menu.");
        }
    }

    public void Pause(bool IsActive)
    {
        if (IsActive)
        {
            _anim.SetTrigger(anim_Unpaused_id);
            StartCoroutine(Deactivate_Pause());
            IsPaused = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
        else
        {
            IsPaused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            _anim.SetTrigger(anim_Unpaused_id);
            _Pause_Menu.SetActive(true);
        }
    }

    private IEnumerator Deactivate_Pause()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (!IsPaused)
        {
            _Pause_Menu.SetActive(false);
        }
    }

    public void Restart_Button()
    {
        Reload();
    }

    public void Main_Menu_Button()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void Exit_Button()
    {

        Application.Quit();
    }
}
