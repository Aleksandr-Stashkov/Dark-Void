using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //Objects
    private Text _txt_SingleScore, _txt_CoopScore;
    private GameObject _pnl_Buttons, _pnl_Score, _txt_Credits;
    //Display triggers
    private bool _isCredits = false;
    private bool _isScores = false;
    //Score data
    private string _singleScore, _coopScore;

    private void Start()
    {
        Debug.Log("Loaded Main_Menu.");
        
        FindObjects();
        //Initial settings
        _txt_Credits.SetActive(false);
        _pnl_Buttons.SetActive(true);
        _pnl_Score.SetActive(false);
        //Getting scores
        _singleScore = PlayerPrefs.GetInt("Single",0).ToString();
        _coopScore = PlayerPrefs.GetInt("Coop",0).ToString();        
    }

    private void FindObjects()
    {
        //variable for children search
        Transform child;

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            switch (child.name)
            {
                case "Credits_text":
                    _txt_Credits = child.gameObject;
                    break;
                case "Buttons_panel":
                    _pnl_Buttons = child.gameObject;
                    break;
                case "Score_panel":
                    _pnl_Score = child.gameObject;
                    break;
            }
        }

        if (_pnl_Score == null)
        {
            Debug.LogWarning("Main Menu could not locate Score panel.");
        }
        else
        {
            for (int i = 0; i < _pnl_Score.transform.childCount; i++)
            {
                child = _pnl_Score.transform.GetChild(i);
                switch (child.name)
                {
                    case "Single_Score_text":
                        _txt_SingleScore = child.GetComponent<Text>();
                        break;
                    case "Coop_Score_text":
                        _txt_CoopScore = child.GetComponent<Text>();
                        break;
                }
            }
        }

        //Check objects
        if (_txt_Credits == null)
        {
            Debug.LogWarning("Main Menu could not locate Credits text.");
        }
        if (_pnl_Buttons == null)
        {
            Debug.LogWarning("Main Menu could not locate Buttons panel.");
        }
        if (_txt_SingleScore == null)
        {
            Debug.LogWarning("Main Menu could not locate Single Score text.");
        }
        if (_txt_CoopScore == null)
        {
            Debug.LogWarning("Main Menu could not locate Coop Score text.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_isScores)
            {
                BackFromPause_Button();
            }
            else
            {
                Exit_Button();
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
    
    public void Start_Button()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void Coop_Button()
    {
        SceneManager.LoadScene("Coop_Level_01");
    }

    public void Scores_Button()
    {
        _isScores = true;
        _pnl_Buttons.SetActive(false);
        _pnl_Score.SetActive(true);       

        _txt_SingleScore.text = _singleScore;
        _txt_CoopScore.text = _coopScore;
    }    

    public void ClearSingleScore_Button()
    {
        PlayerPrefs.SetInt("Single", 0);
        PlayerPrefs.Save();
        _singleScore = "0";
        _txt_SingleScore.text = _singleScore;
    }

    public void ClearCoopScore_Button()
    {
        PlayerPrefs.SetInt("Coop", 0);
        PlayerPrefs.Save();
        _coopScore = "0";
        _txt_CoopScore.text = _coopScore;
    }

    public void BackFromPause_Button()
    {
        _pnl_Score.SetActive(false);
       _isScores = false;
        _pnl_Buttons.SetActive(true);        
    }

    public void Credits_Button()
    {
        if (!_isCredits)
        {
            _isCredits = true;
            StartCoroutine(CreditsDisplay());
        }
    }

    public void Exit_Button()
    {
        Application.Quit();
    }
}