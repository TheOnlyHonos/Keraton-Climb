using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour
{
    [Header("Supply Amount")]
    [SerializeField] private TextMeshProUGUI supplyAmountTMP;

    [Header("Prompt Text Objects")]
    [SerializeField] private TextMeshProUGUI raycastPromptText;
    [SerializeField] private TextMeshProUGUI promptTMP;

    [Space(10)]
    [SerializeField] private GameObject POIObj;
    [SerializeField] private TextMeshProUGUI POITMP;
    [SerializeField] private float timeToShowPromptText = 3f;
    private bool isPOIOpen = false;
    private float timeToHidePromptText;

    [Header("Prompt Text")]
    [SerializeField] private string cantAddMoreSupplyText = "You can't carry any more supplies";
    [SerializeField] private string cantStandHereText = "You can't stand here";
    [SerializeField] private List<string> boonText = new List<string>();

    [Header("Tutorial components")]
    [SerializeField] private GameObject tutorialObj;
    [SerializeField] private TextMeshProUGUI tutorialTMP;
    [SerializeField] private float textSpeed;
    private bool tutorialTextTyped = false;
    private int tutorialIndex;

    [Header("Sprint Indicator")]
    [SerializeField] private TextMeshProUGUI sprintIndicator;

    [Header("Pause Menu Param")]
    [SerializeField] private GameObject pauseMenuObj;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header("Options Container")]
    [SerializeField] private GameObject optionsObj;
    public bool isOptionsOpen;

    private PlayerHealthAndHunger playerHunger;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    private InputManager inputManager;

    private int tempBoonId = 0;

    // Start is called before the first frame update
    void Start()
    {
        isOptionsOpen = false;
        optionsObj.SetActive(false);
        POIObj.SetActive(false);

        playerHunger = GetComponent<PlayerHealthAndHunger>();
        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        if (promptTMP != null && (Time.time >= timeToHidePromptText))
        {
            promptTMP.text = string.Empty;
        }

        if (isPOIOpen)
        {
            inputManager.onFoot.Disable();
            inputManager.onPOIRead.Enable();
            if (inputManager.onPOIRead.Close.triggered)
            {
                HidePOIText();
                inputManager.onPOIRead.Disable();
                inputManager.onFoot.Enable();
            }
        }

        if (tutorialObj.activeSelf)
        {
            HideTutorial();
        }
    }

    public void UpdatePromptMessage(string promptMessage)
    {
        raycastPromptText.text = promptMessage;
    }

    public void UpdateSupplyAmountTMP(string amount)
    {
        supplyAmountTMP.text = amount;
    }

    public void ShowCantAddMoreSupplyTMP()
    {
        promptTMP.text = cantAddMoreSupplyText;

        timeToHidePromptText = Time.time + timeToShowPromptText;
    }

    public void ShowCantStandHere()
    {
        promptTMP.text = cantStandHereText;

        timeToHidePromptText = Time.time + timeToShowPromptText;
    }

    public void ShowTutorial(string tutorialText, int tutorialId)
    {
        StopAllCoroutines();
        tutorialObj.SetActive(true);
        tutorialTMP.text = string.Empty;

        tutorialIndex = tutorialId;
        tutorialObj.SetActive(true);
        StartCoroutine(TypeTutorialLine(tutorialText));
        tutorialTextTyped = true;
    }

    IEnumerator TypeTutorialLine(string tutorialText)
    {
        //type character 1 by 1
        foreach (char c in tutorialText.ToCharArray())
        {
            tutorialTMP.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void HideTutorial()
    {
        switch (tutorialIndex)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            case 5:
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
            default:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (tutorialTextTyped)
                    {
                        tutorialObj.SetActive(false);
                    }
                }
                break;
        }
    }

    public void ShowAndHideSprintIndicator(bool isSprinting)
    {
        if (isSprinting)
        {
            sprintIndicator.text = ">>>";
        } 
        else
        {
            sprintIndicator.text = ">";

        }
    }

    //The boonId functions as follows 0 - no bonuses; 1 - add 1 to max supply amount; 2 - add 20 to max health
    //3 - add 20 to max hunger; 4 - fully heal player; 5 - restore hunger bar to full; 6 - restore both health and hunger to full

    private void ShowBoonText(int id)
    {
        promptTMP.text = boonText[id];

        timeToHidePromptText = Time.time + timeToShowPromptText;
    }

    public void ShowPOIText(string text, int boonId)
    {
        tempBoonId = boonId;

        playerMotor.canMove = false;
        playerLook.canLook = false;

        POITMP.text = text;
        POIObj.SetActive(true);

        isPOIOpen = true;
    }

    public void HidePOIText()
    {
        playerMotor.canMove = true;
        playerLook.canLook = true;

        POIObj.SetActive(false);

        isPOIOpen = false;

        ShowBoonText(tempBoonId);
    }

    public void OpenPauseMenu()
    {
        pauseMenuObj.SetActive(true);
        playerMotor.canMove = false;
        playerLook.canLook = false;
        playerHunger.enableHunger = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuObj.SetActive(false);
        playerMotor.canMove = true;
        playerLook.canLook = true;
        playerHunger.enableHunger = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            resumeButton.interactable = false;
            optionsButton.interactable = false;
            exitButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true;
            optionsButton.interactable = true;
            exitButton.interactable = true;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
