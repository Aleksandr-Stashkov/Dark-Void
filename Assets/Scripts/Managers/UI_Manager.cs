using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    //Managers
    private Game_Manager _Game_manager;
    //UI elements
    private Text _Score, _Score_2;
    private Image _Lives, _Lives_2;
    private Text _GAME_OVER;
    private TMP_Text _Restart;
    //UI assets
    [SerializeField]
    private Sprite[] _liveSprite;
    //Display parameters
    private float t_GameOver_reveal = 2f;

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
                case "Restart_text":
                    _Restart = transform.GetChild(i).GetComponent<TMP_Text>();
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of UI Canvas.");
                    break;
            }
        }
        //UI elements check
        if (_Game_manager == null)
        {
            Debug.LogError("UI manager could not locate Game Manager.");
        }
        if (_Score == null)
        {
            Debug.LogWarning("UI Canvas could not locate Score text.");
        }
        if (_Lives == null)
        {
            Debug.LogWarning("UI Canvas could not locate Lives image.");
        }
        if (_GAME_OVER == null)
        {
            Debug.LogWarning("UI Canvas could not locate GAME OVER text.");
        }
        if(_Restart == null)
        {
            Debug.LogWarning("UI Canvas could not locate Restart text.");
        }
        if (_Game_manager.Check_Coop())
        {
            if (_Score_2 == null)
            {
                Debug.LogWarning("UI Canvas could not locate Score text 2.");
            }
            if (_Lives_2 == null)
            {
                Debug.LogWarning("UI Canvas could not locate Lives image 2.");
            }
        }

        _Score.gameObject.SetActive(false);
        _Lives.gameObject.SetActive(false);
        _GAME_OVER.gameObject.SetActive(false);
        _Restart.gameObject.SetActive(false);
        if (_Game_manager.Check_Coop())
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
        if (_Game_manager.Check_Coop())
        {
            _Score_2.gameObject.SetActive(true);
            _Lives_2.gameObject.SetActive(true);
        }
    }

    public void Score_update(int score)
    {
        if (_Game_manager.Check_Coop())
        {
            _Score.text = score.ToString();
        }
        else
        {
            _Score.text = "Score: " + score.ToString();
        }
    }
    public void Score_update_2(int score)
    {
        _Score_2.text = score.ToString();
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

    public void GAME_OVER(){
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
}
