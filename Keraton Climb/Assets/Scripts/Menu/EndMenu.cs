using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{

    [Header("Main Menu Buttons")]
    [SerializeField] private Button openSurveyButton;
    [SerializeField] private Button returnToMainMenuButton;

    [Header("Survey Parameter")]
    [SerializeField] private string surveyURL;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenSurvey()
    {
        Application.OpenURL(surveyURL);
    }
}
