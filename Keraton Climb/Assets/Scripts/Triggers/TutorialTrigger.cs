using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] [TextArea] private string tutorialText;
    [SerializeField] private int tutorialId;

    private PlayerUI playerUI;

    private void Start()
    {
        playerUI = player.GetComponent<PlayerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerUI.ShowTutorial(tutorialText, tutorialId);
            gameObject.SetActive(false);
        }
    }
}
