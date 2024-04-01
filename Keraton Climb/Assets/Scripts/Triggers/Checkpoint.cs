using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Header("Player Object Parameters")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    private PlayerHealthAndHunger healthAndHunger;

    private void OnTriggerEnter(Collider other)
    {
        healthAndHunger = player.GetComponent<PlayerHealthAndHunger>();
        if (other.CompareTag("Player"))
        {
            healthAndHunger.SetRespawnPoint(respawnPoint);
            healthAndHunger.SaveParameterForReachingCheckpoint(respawnPoint);
        }
    }
}
