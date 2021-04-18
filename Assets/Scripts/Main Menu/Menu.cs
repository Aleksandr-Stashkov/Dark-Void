using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Text _Credits, _Single_score, _Coop_score;
    private GameObject _Buttons, _Score_panel;
    private bool credits_trigger = true;
    private bool scores_trigger = false;
    private string single, coop;

    private void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            switch (transform.GetChild(i).name)
            {
                case "Credits":
                    _Credits = transform.GetChild(i).GetComponent<Text>();
                    break;
                case "Buttons":
                    _Buttons = transform.GetChild(i).gameObject;
                    break;
                case "Score Panel":
                    _Score_panel = transform.GetChild(i).gameObject;
                    break;
            }
        }
        if (_Score_panel == null)
        {
            Debug.LogWarning("Main Menu could not locate Score panel.");
        }
        for (int i = 0; i < _Score_panel.transform.childCount; i++)
        {
            switch (_Score_panel.transform.GetChild(i).name)
            {
                case "Single_Score_text":
                    _Single_score = _Score_panel.transform.GetChild(i).GetComponent<Text>();
                    break;
                case "Coop_Score_text":
                    _Coop_score = _Score_panel.transform.GetChild(i).GetComponent<Text>();
                    break;                
            }
        }

        //Check objects
        if (_Credits == null)
        {
            Debug.LogWarning("Main Menu could not locate Credits text.");
        }
        if (_Buttons == null)
        {
            Debug.LogWarning("Main Menu could not locate Buttons panel.");
        }
        if (_Single_score == null)
        {
            Debug.LogWarning("Main Menu could not locate Single Score text.");
        }
        if (_Coop_score == null)
        {
            Debug.LogWarning("Main Menu could not locate Coop Score text.");
        }

        _Credits.gameObject.SetActive(false);
        _Buttons.gameObject.SetActive(true);
        _Score_panel.gameObject.SetActive(false);

        //Getting scores
        single = PlayerPrefs.GetInt("Single",0).ToString();
        coop = PlayerPrefs.GetInt("Coop",0).ToString();        
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
        _Buttons.SetActive(false);
        _Score_panel.SetActive(true);
        scores_trigger = true;

        _Single_score.text = single;
        _Coop_score.text = coop;
    }

    public void Credits_Button()
    {
        if (credits_trigger)
        {
            credits_trigger = false;
            StartCoroutine(Credits());
        }
    }

    private IEnumerator Credits()
    {
        _Credits.gameObject.SetActive(true);
        yield return new WaitForSeconds(15f);
        _Credits.gameObject.SetActive(false);
        credits_trigger = true;
    }

    public void Exit_Button()
    {
        Application.Quit();
    }

    public void Single_Score_Clear()
    {
        PlayerPrefs.SetInt("Single", 0);
        PlayerPrefs.Save();
        single = "0";
        _Single_score.text = single;
    }

    public void Coop_Score_Clear()
    {
        PlayerPrefs.SetInt("Coop", 0);
        PlayerPrefs.Save();
        coop = "0";
        _Coop_score.text = coop;
    }

    public void Back_Button()
    {
        _Score_panel.SetActive(false);
        scores_trigger = false;
        _Buttons.SetActive(true);        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (scores_trigger)
            {
                Back_Button();
            }
            else
            {
                Exit_Button();
            }
        }
    }
}
