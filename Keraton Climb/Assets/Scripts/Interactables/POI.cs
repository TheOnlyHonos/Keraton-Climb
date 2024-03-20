using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : Interactable
{
    [SerializeField] private GameObject player;
    [SerializeField] [TextArea] private string text;

    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = player.GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //function where interaction designed using code
    protected override void Interact()
    {
        playerUI.ShowPOIText(text);
    }
}
