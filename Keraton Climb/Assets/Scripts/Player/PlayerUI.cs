using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI supplyAmountTMP;
    [SerializeField] private TextMeshProUGUI PromptTMP;
    [SerializeField] private string cantAddMoreSupplyText = "You can't carry any more supplies";
    [SerializeField] private string cantStandHereText = "You can't stand here";
    [SerializeField] private float timeToShowPromptText = 3f;
    private float timeToHidePromptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (PromptTMP != null && (Time.time >= timeToHidePromptText))
        {
            PromptTMP.text = string.Empty;
        }
    }

    public void UpdatePromptMessage(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateSupplyAmountTMP(string amount)
    {
        supplyAmountTMP.text = amount;
    }

    public void ShowCantAddMoreSupplyTMP()
    {
        PromptTMP.text = cantAddMoreSupplyText;

        timeToHidePromptText = Time.time + timeToShowPromptText;
    }

    public void ShowCantStandHere()
    {
        PromptTMP.text = cantStandHereText;

        timeToHidePromptText = Time.time + timeToShowPromptText;
    }
}
