using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    //Managers
    private Game_Manager _Game_manager;
    private bool Coop = false;
    //UI elements
    private Text _Score, _Score_2, _GAME_OVER;
    private Image _Lives, _Lives_2;
    private TMP_Text _Restart;
    private GameObject _Pause_Menu;
    //Pause Menu components
    private Animator _anim_Pause;
    private int anim_Unpaused_id; //Animator parameter id
    private bool Pause_Menu_act; //Interaction with buttons in Pause Menu
    private bool Pause_dir; //Current direction of the Pause switch goal ()
    //UI assets
    [SerializeField]
    private Sprite[] _liveSprite;
    //Display parameters
    private float t_GameOver_reveal = 2f;
    //Score saving
    private int score1, score2;
    private int record;

    void Start()
    {
        _Game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game_Manager>();
        //Identifying child UI elements
        for (int i = 0; i < transform.childCount; i++) {
            switch (transform.GetChild(i).name) {
                case "Score_text":
                    _Score = transform.GetChild(i).GetComponent<Text>();
                    break;
                case "Score_text_2":
                    _Score_2 = transform.GetChild(i).GetComponent<Text>();
                    break;
                case "Lives_image":
                    _Lives = transform.GetChild(i).GetComponent<Image>();
                    break;
                case "Lives_image_2":
                    _Lives_2 = transform.GetChild(i).GetComponent<Image>();
                    break;
                case "GAME_OVER_text":
                    _GAME_OVER = transform.GetChild(i).GetComponent<Text>();
                    break;
                case "Restart_TMP":
                    _Restart = transform.GetChild(i).GetComponent<TMP_Text>();
                    break;
                case "Pause_Menu_panel":
                    _Pause_Menu = transform.GetChild(i).gameObject;
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of UI Canvas.");
                    break;
            }
        }
        if (_Pause_Menu == null)
        {
            Debug.LogWarning("UI Manager could not locate Pause Menu panel.");
        }
        _anim_Pause = _Pause_Menu.GetComponent<Animator>();
        anim_Unpaused_id = Animator.StringToHash("Unpaused");
        //UI elements check
        if (_Game_manager == null)
        {
            Debug.LogError("UI Manager could not locate Game Manager.");
        }
        Coop = _Game_manager.Check_Coop();
        if (_Score == null)
        {
            Debug.LogWarning("UI Manager could not locate Score text.");
        }
        if (_Lives == null)
        {
            Debug.LogWarning("UI Manager could not locate Lives image.");
        }
        if (_GAME_OVER == null)
        {
            Debug.LogWarning("UI Manager could not locate GAME OVER text.");
        }
        if(_Restart == null)
        {
            Debug.LogWarning("UI Manager could not locate Restart text.");
        }        
        if (_anim_Pause == null)
        {
            Debug.LogError("UI Manager could not locate Pause Menu's animator.");
        }        
        if (anim_Unpaused_id == 0)
        {
            Debug.LogWarning("UI Manager could not find Unpaused parameter of the Pause Menu animator.");
        }
        if (Coop)
        {
            if (_Score_2 == null)
            {
                Debug.LogWarning("UI Manager could not locate Score text 2.");
            }
            if (_Lives_2 == null)
            {
                Debug.LogWarning("UI Manager could not locate Lives image 2.");
            }
        }

        //Sending Pause Menu reference to Game Manager
        _Game_manager.Set_UI_Manager(this);
        //Getting record score
        if (Coop)
        {
            record = PlayerPrefs.GetInt("Coop",0);            
        }
        else
        {
            record = PlayerPrefs.GetInt("Single",0);           
        }

        _Score.gameObject.SetActive(false);
        _Lives.gameObject.SetActive(false);
        _GAME_OVER.gameObject.SetActive(false);
        _Restart.gameObject.SetActive(false);
        _Pause_Menu.gameObject.SetActive(false);
        Pause_Menu_act = false;
        Pause_dir = false;
        score1 = 0;
        score2 = 0;
        if (Coop)
        {
            _Score_2.gameObject.SetActive(false);
            _Lives_2.gameObject.SetActive(false);
        }        
    }

    public void Trigger_UI()
    {
        Debug.Log("UI_triggered");
        _Score.gameObject.SetActive(true);
        _Lives.gameObject.SetActive(true);
        if (Coop)
        {
            _Score_2.gameObject.SetActive(true);
            _Lives_2.gameObject.SetActive(true);
        }
    }

    public void Score_update(int new_score)
    {
        score1 = new_score;
        if (Coop)
        {
            _Score.text = new_score.ToString();
        }
        else
        {
            _Score.text = "Score: " + new_score.ToString();
        }
    }
    public void Score_update_2(int new_score)
    {
        score2 = new_score;
        _Score_2.text = new_score.ToString();
    }

    public void Lives_update(int lives)
    {
        if (lives <= 0)
        {
            _Lives.sprite = _liveSprite[0];
        }
        else
        {
            _Lives.sprite = _liveSprite[lives - 1];
        }
    }
    public void Lives_update_2(int lives)
    {
        if (lives <= 0)
        {
            _Lives_2.sprite = _liveSprite[0];
        }
        else
        {
            _Lives_2.sprite = _liveSprite[lives - 1];
        }
    }

    public void GAME_OVER()
    {
        StartCoroutine(GameOver_text());
        _Game_manager.Restart_act();
    }

    //Gradual reveal of the Game Over text
    private IEnumerator GameOver_text()
    {
        float bright = 1f;
        _GAME_OVER.gameObject.SetActive(true);
        while (bright <= t_GameOver_reveal*10)
        {
            _GAME_OVER.color = new Color(bright/(10*t_GameOver_reveal), bright/(10*t_GameOver_reveal), bright/(10*t_GameOver_reveal));
            bright += 1f;
            yield return new WaitForSeconds(0.01f);
        }
        _GAME_OVER.color = new Color(1f,1f,1f);
        _Restart.gameObject.SetActive(true);
    }

    public void Pause(bool IsActive)
    {
        Pause_dir = !Pause_dir;
        if (IsActive)
        {
            _anim_Pause.SetTrigger(anim_Unpaused_id);
            Pause_Menu_act = false;           
            StartCoroutine(Deactivate_Pause());
        }
        else
        {
            Game_Manager.IsPaused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            if (_Pause_Menu.activeSelf)
            {
                _anim_Pause.SetTrigger(anim_Unpaused_id);
            }
            else
            {
                _Pause_Menu.SetActive(true);
            }
            StartCoroutine(Activate_Pause());
        }
    }

    private IEnumerator Activate_Pause()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (Pause_dir)
        {
            Pause_Menu_act = true;
        }
    }

    private IEnumerator Deactivate_Pause()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (!Pause_dir)
        {
            Time.timeScale = 1;
            Game_Manager.IsPaused = false;
            AudioListener.pause = false;
            _Pause_Menu.SetActive(false);
        }
    }

    public void Pause_Resume_Button()
    {
        if (Pause_Menu_act)
        {
            Pause(true);
        }
    }

    public void Pause_Restart_Button()
    {
        if (Pause_Menu_act)
        {
            _Game_manager.Reload();
        }
    }

    public void Pause_Main_Menu_Button()
    {
        if (Pause_Menu_act)
        {
            _Game_manager.Load_Main_Menu();
        }
    }

    public void Pause_Exit_Button()
    {
        if (Pause_Menu_act)
        {
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        if(score1 + score2 > record)
        {
            if (Coop)
            { 
                PlayerPrefs.SetInt("Coop", score1 + score2);
            }
            else
            {
                PlayerPrefs.SetInt("Single", score1);
            }
            PlayerPrefs.Save();
        }
    }
}
