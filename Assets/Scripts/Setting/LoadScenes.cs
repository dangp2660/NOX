using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Setting;
    public void playGame()
    {
        if(SaveManager.HasSaveData())
        {
            SaveManager.DeleteSave();
        }
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void LoadGame()
    {
        if(SaveManager.HasSaveData())
        {
            SceneManager.LoadScene(SaveManager.LoadData().currentMap);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("Don't have save file");
        }
    }
    public void setting()
    {
        Menu.SetActive(false);
        Setting.SetActive(true);
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
    public void back() 
    {
        Setting.SetActive(false);
        Menu.SetActive(true);
    }
}