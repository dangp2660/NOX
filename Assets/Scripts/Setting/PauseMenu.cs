using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject SettingUI;

    public void Home()
    {
        // Tắt PauseMenu trước khi chuyển scene
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    public void Setting()
    {
        SettingUI.SetActive(true);
    }
    public void Back()
    {
        SettingUI.SetActive(false);
    }
    private void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            pauseMenuUI.SetActive(false);
            SettingUI.SetActive(false);
        }  
    }

}
