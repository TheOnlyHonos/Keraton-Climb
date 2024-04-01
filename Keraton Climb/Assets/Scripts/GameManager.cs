using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //Player Values
    public float health;
    public float maxHealth;
    public float hunger;
    public float maxHunger;
    public int supplyAmount;
    public int maxSupplyAmount;
    public int level;

    //For use after load save
    public Vector3 lastPlayerPosition;

    public GameManager()
    {
        this.health = 100;
        this.maxHealth = 100;
        this.hunger = 100;
        this.maxHunger = 100;
        this.supplyAmount = 0;
        this.maxSupplyAmount = 1;
        this.lastPlayerPosition = new Vector3(.0f, .0f, .0f);
        this.level = 0;
    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void resetValues()
    {
        health = 100;
        maxHealth = 100;
        hunger = 100;
        maxHunger = 100;
        supplyAmount = 0;
        maxSupplyAmount = 1;
        lastPlayerPosition = new Vector3(.0f, .0f, .0f);
        level = 0;
    }

    public void setPlayerValues(float currentHealth, float currentMaxHealth, float currentHunger, 
        float currentMaxHunger, int currentSupplyAmount, int currentMaxSupplyAmount, int currentLevel, Vector3 position)
    {
        health = currentHealth;
        maxHealth = currentMaxHealth;
        hunger = currentHunger;
        maxHunger = currentMaxHunger;
        supplyAmount = currentSupplyAmount;
        maxSupplyAmount = currentMaxSupplyAmount;
        level = currentLevel;
        lastPlayerPosition = position;
    }

    public void setPlayerValuesOnCheckpoint(float currentHealth, float currentMaxHealth, float currentHunger, 
        float currentMaxHunger, int currentSupplyAmount, int currentMaxSupplyAmount, int currentLevel, Vector3 position)
    {
        health = currentHealth;
        maxHealth = currentMaxHealth;
        hunger = currentHunger;
        maxHunger = currentMaxHunger;
        supplyAmount = currentSupplyAmount;
        maxSupplyAmount = currentMaxSupplyAmount;
        level = currentLevel;
        lastPlayerPosition = position;
    }

    public void setPlayerPosition(Vector3 pos)
    {
        lastPlayerPosition = pos;
    }

    public Vector3 getPlayerPosition()
    {
        return lastPlayerPosition;
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

    public void SaveGame()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        health = data.health;
        maxHealth = data.maxHealth;
        hunger = data.hunger;
        maxHunger = data.maxHunger;
        supplyAmount = data.supplyAmount;
        maxSupplyAmount = data.maxSupplyAmount;

        Vector3 position = new Vector3(data.lastCheckpointPos[0], data.lastCheckpointPos[1], data.lastCheckpointPos[2]);
        Debug.Log(position);
        lastPlayerPosition = position;

        level = data.level;

        SceneManager.LoadScene(level);
    }
}
