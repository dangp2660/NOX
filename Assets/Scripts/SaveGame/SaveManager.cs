using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    private static string saveFilePath => Application.persistentDataPath + "/save.json";

    public static void SaveGame(Vector3 position, float currentHealth, float maxHealth, float currentDarkEnergy, float maxDarkEnergy, string currentMap, string checkpointName, bool isDefault)
    {
        SaveData data = new SaveData
        {
            playerX = position.x,
            playerY = position.y,
            currentHealth = currentHealth,
            maxHealth = maxHealth,
            currentDarkEnergy = currentDarkEnergy,
            maxDarkEnergy = maxDarkEnergy,
            currentMap = currentMap,
            currentCheckPointName = checkpointName,
            isDefault = isDefault
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved at: " + saveFilePath);
    }

    public static void LoadGame()
    {
        SaveData data = Load();
        if (data != null)
        {
            SceneManager.LoadScene(data.currentMap);
        }
    }

    public static SaveData Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from: " + saveFilePath);
            return data;
        }
        Debug.Log("No save file found.");
        return null;
    }
}

