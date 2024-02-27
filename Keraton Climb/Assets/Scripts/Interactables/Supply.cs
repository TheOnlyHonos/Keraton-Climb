using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : Interactable
{
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //function where interaction designed using code
    protected override void Interact()
    {
        if (player.GetComponent<PlayerHealthAndHunger>().supplyAmount < player.GetComponent<PlayerHealthAndHunger>().maxSupplyAmount)
        {
            player.GetComponent<PlayerHealthAndHunger>().AddSupply();
            gameObject.SetActive(false);
        } else player.GetComponent<PlayerHealthAndHunger>().CantAddMoreSupplyTMP();
    }
}
