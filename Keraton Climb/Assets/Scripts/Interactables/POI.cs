using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : Interactable
{
    [SerializeField] private GameObject player = null;
    [SerializeField] [TextArea] private string text = null;
    [SerializeField] private int boonId = 0;

    private bool hasGivenBoon = false;

    private PlayerUI playerUI;
    private PlayerHealthAndHunger playerHealthAndHunger;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = player.GetComponent<PlayerUI>();
        playerHealthAndHunger = player.GetComponent<PlayerHealthAndHunger>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //The boonId functions as follows 0 - no bonuses; 1 - add 1 to max supply amount; 2 - add 20 to max health
    //3 - add 20 to max hunger; 4 - fully heal player; 5 - restore hunger bar to full; 6 - restore both health and hunger to full


    //function where interaction designed using code
    protected override void Interact()
    {
        //check if player has interacted with POI before to give boon
        if (hasGivenBoon)
        {
            playerUI.ShowPOIText(text, 0);

            playerHealthAndHunger.RecieveBoonFromPOI(0);
        } else
        {
            playerUI.ShowPOIText(text, boonId);

            playerHealthAndHunger.RecieveBoonFromPOI(boonId);
        }

        hasGivenBoon = true;
    }
}
