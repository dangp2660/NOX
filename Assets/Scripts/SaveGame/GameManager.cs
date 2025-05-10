using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Data")]
    public GameObject player;
    public int currentCheckpoint;
    public EnemyManager enemyManager;
    private void Awake()
    {
        // Singleton Pattern để đảm bảo chỉ có một GameManager tồn tại
        player = GameObject.FindGameObjectWithTag("Player");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadGameData();
    }

    // Hàm gọi khi đạt checkpoint
    public void SaveAtCheckpoint(string currentMap, Vector3 position, string checkPointName)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            Vector3 pos = player.transform.position;
            SaveManager.SaveGame(
                pos,
                player.GetComponent<Damageable>().CurrentHealth,
                player.GetComponent<DarkEnergyManager>().CurrentDarkEnergy,
                player.GetComponent<Damageable>().getMaxHealth(),
                player.GetComponent<DarkEnergyManager>().MaxDarkEnergy,
                currentMap,checkPointName
            );
            Debug.Log("Game saved at checkpoint: " + currentCheckpoint);
        }
        else
        {
            Debug.LogError("Player not found! Unable to save.");
        }
    }

    // Hàm tự động load khi vào game từ Menu
    private void LoadGameData()
    {
        SaveData data = SaveManager.LoadData();
        if (data != null)
        {
            SceneManager.LoadScene(data.currentMap);
            Debug.Log("Game loaded from map: " + data.currentMap);

            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = data.playerPos;
                player.GetComponent<Damageable>().CurrentHealth = data.currentHP;
                player.GetComponent<Damageable>().setMaxHP(data.maxHP);
                player.GetComponent<DarkEnergyManager>().CurrentDarkEnergy = data.currentDarkEnergy;
                player.GetComponent<DarkEnergyManager>().setDarkEnergy(data.maxDarkEnergy);
                enemyManager.respawnEnemy(data.checkPointID);
                Debug.Log("Player data loaded successfully."); 
            }
            else
            {
                Debug.LogError("Player not found during load.");
            }   
        }
        else
        {
            Debug.Log("No previous save found. Starting new game.");
        }
    }

    // Xóa file save
    public void DeleteSaveData()
    {
        SaveManager.DeleteSave();
        Debug.Log("Save data deleted.");
    }

    // Khởi động game mới từ Menu
    public void StartNewGame()
    {
        DeleteSaveData();
        SceneManager.LoadScene("Map1");  // Thay bằng tên map đầu tiên của bạn
        Debug.Log("Starting a new game.");
    }

    // Tiếp tục từ save data
    public void ContinueGame()
    {
        SaveData data = SaveManager.LoadData();
        if (data != null)
        {
            SceneManager.LoadScene(data.currentMap);
            Debug.Log("Continuing from saved game.");
        }
        else
        {
            Debug.Log("No save data found.");
        }
    }
}
