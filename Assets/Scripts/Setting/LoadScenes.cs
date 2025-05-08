using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    
    public void playGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        SaveData data = SaveGameManager.Load();
        if (data != null)
        {
            // Sau khi load dữ liệu, bạn sẽ chuyển đến scene chính
            SceneManager.LoadScene(data.currentMap); // Đảm bảo sceneName lưu trữ tên scene đúng

            // Sau đó, trong scene chính, bạn sẽ khôi phục trạng thái của người chơi
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            RespawnScript respawnScript = player.GetComponent<RespawnScript>();

            // Khôi phục thông tin từ dữ liệu đã lưu
            respawnScript.LoadFromData(data);
        }
        else
        {
            Debug.Log("Dont't have save Game");
            return;
        }
    }
    public void setting()
    {
        SceneManager.LoadScene(1);
    }
    public void menuGame()
    {
        SceneManager.LoadScene(0);
    }

    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadScene(int id)
    {
        SceneManager.LoadSceneAsync(id);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
