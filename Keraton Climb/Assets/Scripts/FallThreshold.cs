using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThreshold : MonoBehaviour
{
    [Header("Player Object Parameters")]
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealthAndHunger>().Die(.5f);
        }
    }
}
