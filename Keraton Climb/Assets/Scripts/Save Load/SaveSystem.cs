using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

public static class SaveSystem
{
    static string qwy781f178fhe8fgh = "vyCudgHmP0RcBA9";

    public static void SavePlayer (GameManager manager)
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";

        PlayerData data = new PlayerData(manager);
        Debug.Log(data.supplyAmount + " " + data.lastCheckpointPos[0] + ", " + data.lastCheckpointPos[1] + ", " + data.lastCheckpointPos[2]);

        string json = JsonUtility.ToJson(data, false);
        var encryptedJson = EncryptDecrypt(json);

        File.WriteAllText(filePath, encryptedJson);

        Debug.Log("Saving game");
    }

    public static PlayerData LoadPlayer ()
    {
        string filePath = Application.persistentDataPath + "/keraton_climb.sav";
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);

            PlayerData data = JsonUtility.FromJson<PlayerData>(EncryptDecrypt(json));

            Debug.Log("Loading game");
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

    private static string EncryptDecrypt(string data)
    {
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ qwy781f178fhe8fgh[i % qwy781f178fhe8fgh.Length]);
        }

        return result;
    }
}
