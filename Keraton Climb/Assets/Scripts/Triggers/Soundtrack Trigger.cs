using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackTrigger : MonoBehaviour
{
    [Header("Player Object Parameters")]
    [SerializeField] private GameObject musicManager;
    
    //bool for checking if trigger is for the music to start or for the music to stop
    [SerializeField] private bool isStartMusicTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isStartMusicTrigger)
            {
                musicManager.GetComponent<MusicManager>().startMusic();
            } else musicManager.GetComponent<MusicManager>().stopMusic();

        }
    }
}
