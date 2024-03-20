using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
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
}
