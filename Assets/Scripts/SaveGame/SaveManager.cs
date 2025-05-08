using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/saveData.json";

    public static void SaveGame(Vector3 playerPos, float currentHP, float currentDarkEnergy, float maxHP, float maxDarkEnergy, string currentMap)
    {
        SaveData data = new SaveData(playerPos, currentHP, currentDarkEnergy, maxHP, maxDarkEnergy, currentMap);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
       Debug.Log(data.playerPos);
    }

    public static SaveData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded successfully.");
            return data;
        }
        Debug.Log("No save file found.");
        return null;
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }

    public static bool HasSaveData() { return File.Exists(savePath); }
}
