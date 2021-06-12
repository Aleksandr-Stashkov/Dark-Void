using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _pnl_PauseMenu;
    private Animator _anim_Pause;
    private int _anim_ID_Pause; //Animator parameter id
    private float _anim_Length;
    private bool _isPauseMenuActive; //Interaction with buttons in Pause Menu
    private bool _isBeingPaused; //Current direction of the Pause change (for correct animation function)

    void Start()
    {
        if (_pnl_PauseMenu == null)
        {
            Debug.LogError("Pause Menu was not assigned to the script.");
        }
        else
        {
            _anim_Pause = _pnl_PauseMenu.GetComponent<Animator>();
            if (_anim_Pause == null)
            {
                Debug.LogError("Pause Menu could not locate its Animator.");
            }
            else
            {
                _anim_ID_Pause = Animator.StringToHash("Pause");
                _anim_Length = _anim_Pause.runtimeAnimatorController.animationClips[0].length;
            }
        }

        if (_anim_ID_Pause == 0)
        {
            Debug.LogError("Pause Menu could not find Pause parameter of its Animator.");
        }
        if (_anim_Length <= 0)
        {
            Debug.LogAssertion("Animation length for the Pause Menu is invalid.");
        }

        _pnl_PauseMenu.SetActive(false);
        _isPauseMenuActive = false;
        _isBeingPaused = false;
    }

    private IEnumerator ActivatePause()
    {
        yield return new WaitForSecondsRealtime(_anim_Length);
        if (_isBeingPaused)
        {
            _isPauseMenuActive = true;
        }
    }

    private IEnumerator DeactivatePause()
    {
        yield return new WaitForSecondsRealtime(_anim_Length);
        if (!_isBeingPaused)
        {
            Time.timeScale = 1;
            GameManager.isPaused = false;
            AudioListener.pause = false;
            _pnl_PauseMenu.SetActive(false);
        }
    }

    public void SetGameManager(GameManager gameManager)
    {
        if (gameManager == null)
        {
            Debug.LogAssertion("Pause Menu script was handled an empty Game Manager.");
        }
        else
        {
            _gameManager = gameManager;
        }
    }

    public void Pause(bool isActive)
    {
        _isBeingPaused = !_isBeingPaused;

        if (isActive)
        {
            _anim_Pause.SetTrigger(_anim_ID_Pause);
            _isPauseMenuActive = false;

            StartCoroutine(DeactivatePause());
        }
        else
        {
            GameManager.isPaused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            //Differnce between reverting the animation or starting it
            if (_pnl_PauseMenu.activeSelf)
            {
                _anim_Pause.SetTrigger(_anim_ID_Pause);
            }
            else
            {
                _pnl_PauseMenu.SetActive(true);
            }

            StartCoroutine(ActivatePause());
        }
    }

    public void PauseResumeButton()
    {
        if (_isPauseMenuActive)
        {
            Pause(true);
        }
    }

    public void PauseRestartButton()
    {
        if (_isPauseMenuActive)
        {
            _gameManager.ReloadScene();
        }
    }

    public void PauseMainMenuButton()
    {
        if (_isPauseMenuActive)
        {
            _gameManager.LoadMainMenu();
        }
    }

    public void PauseExitButton()
    {
        if (_isPauseMenuActive)
        {
            Application.Quit();
        }
    }
}
