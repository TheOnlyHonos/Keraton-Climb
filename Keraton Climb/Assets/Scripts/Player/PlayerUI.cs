using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Pause Menu Param")]
    [SerializeField] private GameObject pauseMenuObj;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header("Options Container")]
    [SerializeField] private GameObject optionsObj;
    public bool isOptionsOpen;

    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        isOptionsOpen = false;
        optionsObj.SetActive(false);
        POIObj.SetActive(false);

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

        if(isPOIOpen)
        {
            inputManager.onFoot.Disable();
            inputManager.onPOIRead.Enable();
            if (inputManager.onPOIRead.Close.triggered)
            {
                playerMotor.canMove = true;
                playerLook.canLook = true;

                POIObj.SetActive(false);

                isPOIOpen = false;

                inputManager.onPOIRead.Disable();
                inputManager.onFoot.Enable();
            }
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

    public void ShowPOIText(string text)
    {
        playerMotor.canMove = false;
        playerLook.canLook = false;

        POITMP.text = text;
        POIObj.SetActive(true);

        isPOIOpen = true;
    }

    public void OpenPauseMenu()
    {

        pauseMenuObj.SetActive(true);
        playerMotor.canMove = false;
        playerLook.canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuObj.SetActive(false);
        playerMotor.canMove = true;
        playerLook.canLook = true;

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
