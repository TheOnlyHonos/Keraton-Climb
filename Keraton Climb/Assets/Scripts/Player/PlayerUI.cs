using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI supplyAmountTMP;
    [SerializeField] private TextMeshProUGUI cantAddMoreSupplyTMP;
    [SerializeField] private string cantAddMoreSupplyText = "You can't carry any more supplies";
    [SerializeField] private float timeToShowCantAddMoreSupplyTMP = 3f;
    private float timeToHideCantAddMoreSupplyTMP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (cantAddMoreSupplyTMP != null && (Time.time >= timeToHideCantAddMoreSupplyTMP))
        {
            cantAddMoreSupplyTMP.text = string.Empty;
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
        cantAddMoreSupplyTMP.text = cantAddMoreSupplyText;

        timeToHideCantAddMoreSupplyTMP = Time.time + timeToShowCantAddMoreSupplyTMP;
    }
}
