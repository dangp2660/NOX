using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    private static string savePath => Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public static SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded successfully!");

            // Nếu có dữ liệu hợp lệ, chuyển sang scene game
            if (data != null)
            {
                // Giả sử scene game có buildIndex = 1 hoặc tên là "GameScene"
                Debug.Log("Data");
                SceneManager.LoadSceneAsync(data.currentMap);
                
            }
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }



    public static void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
