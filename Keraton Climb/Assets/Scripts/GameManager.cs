using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Player Values
    public float health = 100;
    public float hunger = 100;
    public int supplyAmount = 0;

    //For use after load save
    public Transform lastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void setPlayerValues(float currentHealth, float currentHunger, int currentSupplyAmount)
    {
        health = currentHealth;
        hunger = currentHunger;
        supplyAmount = currentSupplyAmount;
    }
    public float getPlayerHealth()
    {
        return health;
    }

    public float getPlayerHunger()
    {
        return hunger;
    }

    public int getPlayerSupplyAmount()
    {
        return supplyAmount;
    }
}
