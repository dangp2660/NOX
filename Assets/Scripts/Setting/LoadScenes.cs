using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    [SerializeField] private string LoadScene;
    public void changeScene()
    {
        SceneManager.LoadScene(LoadScene);
    }
}
