using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (GameManager manager)
    {
        Debug.Log("SaveSystem SaveGame");

        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        FileStream stream = new FileStream(filePath, FileMode.Create);

        PlayerData data = new PlayerData(manager);

        Debug.Log(data.supplyAmount + " " + data.lastCheckpointPos[0] + ", " + data.lastCheckpointPos[1] + ", " + data.lastCheckpointPos[2]);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save succesful");
    }

    public static PlayerData LoadPlayer ()
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
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
