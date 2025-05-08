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