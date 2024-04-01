using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //Player Values
    public float health;
    public float maxHealth;
    public float hunger;
    public float maxHunger;
    public int supplyAmount;
    public int maxSupplyAmount;
    public int level;

    //For use after load save
    public float[] lastCheckpointPos;

    public PlayerData (GameManager manager)
    {
        health = manager.health;
        maxHealth = manager.maxHealth;
        hunger = manager.hunger;
        maxHunger = manager.maxHunger;
        supplyAmount = manager.supplyAmount;
        maxSupplyAmount = manager.maxSupplyAmount;
        level = manager.level;

        lastCheckpointPos = new float[3];
        lastCheckpointPos[0] = manager.lastPlayerPosition.x;
        lastCheckpointPos[1] = manager.lastPlayerPosition.y;
        lastCheckpointPos[2] = manager.lastPlayerPosition.z;
    }
}
