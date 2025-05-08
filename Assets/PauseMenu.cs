using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;

    public void Home()
    {
        // Tắt PauseMenu trước khi chuyển scene
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        // Đảm bảo rằng menu luôn tắt ở scene "Menu"
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            pauseMenuUI.SetActive(false);
        }
    }
}
