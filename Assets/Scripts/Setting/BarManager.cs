using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarManager : MonoBehaviour
{
    public GameObject bar;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.CompareTo("Menu")==0 || 
            SceneManager.GetActiveScene().buildIndex == 0)
        {
            bar.SetActive(false);
            return;
        }
        bar.SetActive(true);
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
