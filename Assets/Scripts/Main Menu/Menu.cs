using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Text _txt_SingleScore, _txt_CoopScore;
    private GameObject _pnl_Buttons, _pnl_Score, _txt_Credits;
    
    private bool _isCredits = false;
    private bool _isScores = false;
    private string _singleScore, _coopScore;

    private void Start()
    {
        Debug.Log("Loaded Main Menu.");
        
        FindObjects();

        _txt_Credits.SetActive(false);
        _pnl_Buttons.SetActive(true);
        _pnl_Score.SetActive(false);

        _singleScore = PlayerPrefs.GetInt("Single",0).ToString();
        _coopScore = PlayerPrefs.GetInt("Coop",0).ToString();        
    }

    private void FindObjects()
    {
        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            switch (child.name)
            {
                case "Credits Text":
                    _txt_Credits = child.gameObject;
                    break;
                case "Buttons Panel":
                    _pnl_Buttons = child.gameObject;
                    break;
                case "Score Panel":
                    _pnl_Score = child.gameObject;
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of Canvas.");
                    break;
            }
        }

        if (_pnl_Score == null)
        {
            Debug.LogError("Main Menu could not locate Score Panel.");
        }
        else
        {
            for (int i = 0; i < _pnl_Score.transform.childCount; i++)
            {
                child = _pnl_Score.transform.GetChild(i);
                switch (child.name)
                {
                    case "Single Score Text":
                        _txt_SingleScore = child.GetComponent<Text>();
                        break;
                    case "Coop Score Text":
                        _txt_CoopScore = child.GetComponent<Text>();
                        break;
                    default:
                        Debug.LogWarning("There is an unrecognized child of Score Panel.");
                        break;
                }
            }
        }

        if (_txt_Credits == null)
        {
            Debug.LogError("Main Menu could not locate Credits Text.");
        }
        if (_pnl_Buttons == null)
        {
            Debug.LogError("Main Menu could not locate Buttons Panel.");
        }
        if (_txt_SingleScore == null)
        {
            Debug.LogError("Main Menu could not locate Single Score Text.");
        }
        if (_txt_CoopScore == null)
        {
            Debug.LogError("Main Menu could not locate Coop Score Text.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_isScores)
            {
                BackFromPauseButton();
            }
            else
            {
                ExitButton();
            }
        }
    }

    private IEnumerator CreditsDisplay()
    {
        _txt_Credits.SetActive(true);
        yield return new WaitForSeconds(15f);
        _txt_Credits.SetActive(false);
        _isCredits = false;
    }
    
    public void SinglePlayerButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CoopButton()
    {
        SceneManager.LoadScene(2);
    }

    public void ScoresButton()
    {
        _isScores = true;
        _pnl_Buttons.SetActive(false);
        _pnl_Score.SetActive(true);       

        _txt_SingleScore.text = _singleScore;
        _txt_CoopScore.text = _coopScore;
    }    

    public void ClearSingleScoreButton()
    {
        PlayerPrefs.SetInt("Single", 0);
        PlayerPrefs.Save();
        _singleScore = "0";
        _txt_SingleScore.text = _singleScore;
    }

    public void ClearCoopScoreButton()
    {
        PlayerPrefs.SetInt("Coop", 0);
        PlayerPrefs.Save();
        _coopScore = "0";
        _txt_CoopScore.text = _coopScore;
    }

    public void BackFromPauseButton()
    {
        _pnl_Score.SetActive(false);
        _isScores = false;
        _pnl_Buttons.SetActive(true);        
    }

    public void CreditsButton()
    {
        if (!_isCredits)
        {
            _isCredits = true;
            StartCoroutine(CreditsDisplay());
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}