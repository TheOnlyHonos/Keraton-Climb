using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Player Values
    public float health = 100;
    public float maxHealth = 100;
    public float hunger = 100;
    public float maxHunger = 100;
    public int supplyAmount = 0;
    public int maxSupplyAmount = 1;

    //For use after load save
    public Transform lastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void setPlayerValues(float currentHealth, float currentMaxHealth, float currentHunger, float currentMaxHunger, int currentSupplyAmount, int currentMaxSupplyAmount)
    {
        health = currentHealth;
        maxHealth = currentMaxHealth;
        hunger = currentHunger;
        maxHunger = currentMaxHunger;
        supplyAmount = currentSupplyAmount;
        maxSupplyAmount = currentMaxSupplyAmount;
    }
    public float getPlayerHealth()
    {
        return health;
    }

    public float getPlayerMaxHealth()
    {
        return maxHealth;
    }

    public float getPlayerHunger()
    {
        return hunger;
    }

    public float getPlayerMaxHunger()
    {
        return maxHunger;
    }

    public int getPlayerSupplyAmount()
    {
        return supplyAmount;
    }

    public int getPlayerMaxSupplyAmount()
    {
        return maxSupplyAmount;
    }
}
