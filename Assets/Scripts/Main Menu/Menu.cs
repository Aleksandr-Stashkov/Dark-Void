using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Text _Credits;
    private bool credits_trigger = true;

    private void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            switch (transform.GetChild(i).name)
            {
                case "Credits":
                    _Credits = transform.GetChild(i).GetComponent<Text>();
                    break;
            }
        }
        if (_Credits == null)
        {
            Debug.LogWarning("Main Menu could not locate Credits text.");
        }
    }

    public void Start_Button()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void Coop_Button()
    {
        SceneManager.LoadScene("Coop_Level_01");
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Exit_Button();
        }
    }
}
