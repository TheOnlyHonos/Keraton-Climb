using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header("Options Container")]
    [SerializeField] private GameObject optionsObj;
    public bool isOptionsOpen;

    void Start()
    {
        isOptionsOpen = false;
        optionsObj.SetActive(false);
    }

    public void Continue()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void NewGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void OpenOptionsMenu()
    {
        isOptionsOpen = true;
        optionsObj.SetActive(true);
        SetButtonsInteractable();
    }

    public void CloseOptionsMenu()
    {
        isOptionsOpen = false;
        optionsObj.SetActive(false);
        SetButtonsInteractable();
    }

    public void SetButtonsInteractable()
    {
        if (isOptionsOpen)
        {
            continueButton.interactable = false;
            newGameButton.interactable = false;
            optionsButton.interactable = false;
            exitButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
            newGameButton.interactable = true;
            optionsButton.interactable = true;
            exitButton.interactable = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
