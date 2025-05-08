using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{

    public void playGame()
    {
        if(SaveManager.HasSaveData())
        {
            SaveManager.DeleteSave();
        }
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        if(SaveManager.HasSaveData())
        {
            SceneManager.LoadScene(SaveManager.LoadData().currentMap);
        }
        else
        {
            Debug.Log("Don't have save file");
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