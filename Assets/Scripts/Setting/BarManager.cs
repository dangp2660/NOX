using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarManager : MonoBehaviour
{
    public GameObject bar;

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name + ":" + currentScene.buildIndex);

        // Kiểm tra bar có được gán hay không
        if (bar == null)
        {
            Debug.LogWarning("Bar object is not assigned!");
            return;
        }

        // Ẩn bar nếu ở màn hình menu
        if (currentScene.name == "Menu" || currentScene.buildIndex == 0)
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
        }
    }
    private void Awake()
    {
        // Lưu trữ cảnh hiện tại vào biến
        
    }
    public void EnableSignal()
    {
        this.gameObject.SetActive(true);
    }
    public void DisableSignal()
    {
        this.gameObject.SetActive(false);
    }
}
