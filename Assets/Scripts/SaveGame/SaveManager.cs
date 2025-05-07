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
                SceneManager.LoadSceneAsync(data.currentMap);
                GameObject manager = new GameObject("LoadManager");
                SaveManager saveManager = manager.AddComponent<SaveManager>();
                saveManager.StartCoroutine(saveManager.RestoreGameData(data));
            }

            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }

    IEnumerator RestoreGameData(SaveData data)
    {
        yield return null;

        //find game object
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        GameObject PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        RespawnScript respawn = GameObject.FindAnyObjectByType<RespawnScript>();
        if (Player != null)
        {
            // restore position
            Player.transform.position = new Vector3(data.playerX, data.playerY, 0);
            // restore health and dark energy
            Damageable health = Player.GetComponent<Damageable>();
            DarkEnergyManager energyManager = new DarkEnergyManager();

            if(health != null)
            {
                health.CurrentHealth = data.currentHealth;
            }
            if(energyManager != null)
            {
                energyManager.CurrentDarkEnergy = data.currentEnergy;
            }
        }
        if (PlayerManager != null)
        {
            PlayerManager.GetComponent<PlayerSwitch>().isDefault = data.isDefault;
        }
        if (respawn != null)
        {
            GameObject checkpoint = GameObject.Find(data.currentCheckPointName);

        }

    }


    public static void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
