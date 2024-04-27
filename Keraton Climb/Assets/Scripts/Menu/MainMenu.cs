using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject continueButtonObj;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header("Survey Parameter")]
    [SerializeField] private string surveyURL;
    [SerializeField] private Button surveyButton;

    [Header("Options Container")]
    [SerializeField] private GameObject optionsObj;
    public bool isOptionsOpen;

    [Header("Game Manager")]
    private GameManager gameManager;

    void Start()
    {
        isOptionsOpen = false;
        optionsObj.SetActive(false);

        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        bool saveFileExist = CheckForSaveFile();

        Debug.Log(saveFileExist);

        if (saveFileExist)
        {
            continueButtonObj.SetActive(true);
        } else continueButtonObj.SetActive(false);
    }

    public bool CheckForSaveFile()
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            Debug.LogError("Save file not found in" + filePath);
            return false;
        }
    }

    public void Continue()
    {
        gameManager.LoadGame();
    }

    public void NewGame()
    {
        SaveSystem.DeleteSaveData();

        gameManager.resetValues();

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
            surveyButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
            newGameButton.interactable = true;
            optionsButton.interactable = true;
            exitButton.interactable = true;
            surveyButton.interactable = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }
}
