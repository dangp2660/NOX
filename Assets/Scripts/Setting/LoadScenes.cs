using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void setting()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void menuGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
