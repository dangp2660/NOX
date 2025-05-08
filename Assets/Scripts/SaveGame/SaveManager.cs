using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/saveData.json";
        
    }

    public static void SaveGame(Vector3 playerPos, 
        float currentHP, 
        float currentDarkEnergy, 
        float maxHP, float 
        maxDarkEnergy, 
        string currentMap)
    {
        if (File.Exists(savePath))
        {
            SaveData data = new SaveData(playerPos,
                currentHP,
                currentDarkEnergy,
                maxHP,
                maxDarkEnergy,
                currentMap);
            string json =  JsonUtility.ToJson(data);
            File.WriteAllText(savePath, json);
            Debug.Log("save path: " + savePath);
        }
    }

    public static SaveData LoadData()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Load success");
            return data;
        }
        else
        {
            Debug.Log("Don't have file");
            return null;
        }
    }
}
