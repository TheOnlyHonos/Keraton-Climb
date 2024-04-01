using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (GameManager manager)
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";

        PlayerData data = new PlayerData(manager);
        Debug.Log(data.supplyAmount + " " + data.lastCheckpointPos[0] + ", " + data.lastCheckpointPos[1] + ", " + data.lastCheckpointPos[2]);

        string json = JsonUtility.ToJson(data, false);
        File.WriteAllText(filePath, json);
    }

    public static PlayerData LoadPlayer ()
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            Debug.Log(data.health + " " +  data.hunger + " " + data.lastCheckpointPos[0] + ", " + data.lastCheckpointPos[1] + ", " + data.lastCheckpointPos[2]);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + filePath);
            return null;
        }
    }

    public static void DeleteSaveData()
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        File.Delete(filePath);
    }
}
