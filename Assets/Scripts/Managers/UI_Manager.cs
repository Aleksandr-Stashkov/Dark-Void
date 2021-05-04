using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isCoop = false;
    //Main UI elements
    private GameObject _pnl_Controls, _tmp_Restart, _tmp_NewRecord;
    private Text _txt_Score1, _txt_Score2, _txt_GameOver;
    private Image _img_Lives1, _img_Lives2;
    //Pause Menu
    private GameObject _pnl_PauseMenu;
    private Animator _anim_Pause;
    private int _anim_ID_Pause; //Animator parameter id
    private float _anim_Length;
    private bool _isPauseMenuActive; //Interaction with buttons in Pause Menu
    private bool _isBeingPaused; //Current direction of the Pause change (for correct animation function)
    
    [SerializeField]
    private Sprite[] _livesSprites;
    
    private float _gameOverRevealDuration = 2f;

    private int _score1, _score2;
    private int _record;
    private bool _isNewRecord = false;

    void Start()
    {
        FindElements();
        GetRecord();
        Initial_UI_Setting();

        if (_isCoop)
        {
            StartCoroutine(ControlsDisplay());
        }
    }

    private void FindElements()
    {
        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            switch (child.name)
            {
                case "Score Text":
                    _txt_Score1 = child.GetComponent<Text>();
                    break;
                case "Score2 Text":
                    _txt_Score2 = child.GetComponent<Text>();
                    break;
                case "Lives Image":
                    _img_Lives1 = child.GetComponent<Image>();
                    break;
                case "Lives2 Image":
                    _img_Lives2 = child.GetComponent<Image>();
                    break;
                case "Game Over Text":
                    _txt_GameOver = child.GetComponent<Text>();
                    break;
                case "Restart TMP":
                    _tmp_Restart = child.gameObject;
                    break;
                case "New Record TMP":
                    _tmp_NewRecord = child.gameObject;
                    break;
                case "Pause Menu Panel":
                    _pnl_PauseMenu = child.gameObject;
                    break;
                case "Controls Panel":
                    _pnl_Controls = child.gameObject;
                    break;                
                default:
                    Debug.LogWarning("There is an unrecognized child of Canvas.");
                    break;
            }
        }

        if (_pnl_PauseMenu == null)
        {
            Debug.LogError("UI Manager could not locate Pause Menu Panel.");
        }
        else
        {
            _anim_Pause = _pnl_PauseMenu.GetComponent<Animator>();
            _anim_ID_Pause = Animator.StringToHash("Pause");
            _anim_Length = _anim_Pause.runtimeAnimatorController.animationClips[0].length;
        }
        
        if (_gameManager == null)
        {
            Debug.LogError("UI Manager could not locate Game Manager.");
        }
        else
        {
            _isCoop = _gameManager.IsCoop();
        }

        CheckElements();
    }

    private void CheckElements()
    {
        if (_txt_Score1 == null)
        {
            Debug.LogError("UI Manager could not locate Score1 Text.");
        }
        if (_img_Lives1 == null)
        {
            Debug.LogError("UI Manager could not locate Lives1 Image.");
        }
        if (_txt_GameOver == null)
        {
            Debug.LogError("UI Manager could not locate Game Over Text.");
        }
        if (_tmp_Restart == null)
        {
            Debug.LogError("UI Manager could not locate Restart Text.");
        }
        if (_tmp_NewRecord == null)
        {
            Debug.LogError("UI Manager could not locate New Record Text.");
        }
        if (_anim_Pause == null)
        {
            Debug.LogError("UI Manager could not locate Pause Menu's Animator.");
        }
        if (_anim_ID_Pause == 0)
        {
            Debug.LogError("UI Manager could not find Pause parameter of the Pause Menu Animator.");
        }
        if (_anim_Length <= 0)
        {
            Debug.LogAssertion("Animation length for the Pause Menu is invalid.");
        }
        if (_isCoop)
        {
            if (_txt_Score2 == null)
            {
                Debug.LogError("UI Manager could not locate Score2 Text.");
            }
            if (_img_Lives2 == null)
            {
                Debug.LogError("UI Manager could not locate Lives2 Image.");
            }
            if (_pnl_Controls == null)
            {
                Debug.LogError("UI Manager could not locate Controls Panel.");
            }
        }
    }

    private void GetRecord()
    {
        if (_isCoop)
        {
            _record = PlayerPrefs.GetInt("Coop", 0);
        }
        else
        {
            _record = PlayerPrefs.GetInt("Single", 0);
        }
    }

    private void Initial_UI_Setting()
    {
        _txt_Score1.gameObject.SetActive(false);
        _img_Lives1.gameObject.SetActive(false);
        _txt_GameOver.gameObject.SetActive(false);
        _tmp_Restart.SetActive(false);
        _tmp_NewRecord.SetActive(false);
        _pnl_PauseMenu.SetActive(false);
        _isPauseMenuActive = false;
        _isBeingPaused = false;
        _isNewRecord = false;
        _score1 = 0;
        _score2 = 0;

        if (_isCoop)
        {
            _txt_Score2.gameObject.SetActive(false);
            _img_Lives2.gameObject.SetActive(false);
            _pnl_Controls.SetActive(false);
        }
    }    

    private void UpdateRecord(int newRecord)
    {
        if (!_isNewRecord)
        {
            _isNewRecord = true;
            StartCoroutine(NewRecordDisplay());            
        }

        if (_isCoop)
        {
            PlayerPrefs.SetInt("Coop", newRecord);
        }
        else
        {
            PlayerPrefs.SetInt("Single", newRecord);
        }
        PlayerPrefs.Save();       
    }

    private IEnumerator NewRecordDisplay()
    {
        _tmp_NewRecord.SetActive(true);
        yield return new WaitForSeconds(2f);
        _tmp_NewRecord.SetActive(false);
    }

    private IEnumerator ControlsDisplay()
    {
        _pnl_Controls.SetActive(true);
        yield return new WaitForSeconds(120f);
        _pnl_Controls.SetActive(false);
    }
    
    private IEnumerator GameOverReveal()
    {
        float step = 1f;
        float fullBrightness = _gameOverRevealDuration * 10f;

        _txt_GameOver.gameObject.SetActive(true);
        while (step <= fullBrightness)
        {
            _txt_GameOver.color = new Color(step / fullBrightness, step / fullBrightness, step / fullBrightness);
            step += 1f;
            yield return new WaitForSeconds(0.01f);
        }

        _txt_GameOver.color = new Color(1f, 1f, 1f);
        _tmp_Restart.SetActive(true);
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
            Debug.LogAssertion("UI Manager was handled an empty Game Manager.");
        }
        else
        {
            _gameManager = gameManager;
        }
    }

    public void Trigger_UI()
    {        
        _txt_Score1.gameObject.SetActive(true);
        _img_Lives1.gameObject.SetActive(true);
        if (_isCoop)
        {
            _txt_Score2.gameObject.SetActive(true);
            _img_Lives2.gameObject.SetActive(true);
        }

        Debug.Log("UI triggered.");
    }

    public void UpdateScore1(int newScore)
    {
        _score1 = newScore;
        if (_isCoop)
        {
            _txt_Score1.text = _score1.ToString();
        }
        else
        {
            _txt_Score1.text = "Score: " + _score1.ToString();
        }

        if(_score1+_score2 > _record)
        {
            UpdateRecord(_score1+_score2);
        }
    }
    public void UpdateScore2(int newScore)
    {
        _score2 = newScore;
        _txt_Score2.text = _score2.ToString();

        if (_score1 + _score2 > _record)
        {
            UpdateRecord(_score1+_score2);
        }
    }

    public void UpdateLives1(int lives)
    {
        if (lives <= 0)
        {
            _img_Lives1.sprite = _livesSprites[0];
        }
        else
        {
            _img_Lives1.sprite = _livesSprites[lives - 1];
        }
    }
    public void UpdateLives2(int lives)
    {
        if (lives <= 0)
        {
            _img_Lives2.sprite = _livesSprites[0];
        }
        else
        {
            _img_Lives2.sprite = _livesSprites[lives - 1];
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverReveal());
        _gameManager.AllowRestart();
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