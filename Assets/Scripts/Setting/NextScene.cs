using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string SceneName;
    void Start()
    {
        SceneManager.LoadScene(SceneName);
    }


}
