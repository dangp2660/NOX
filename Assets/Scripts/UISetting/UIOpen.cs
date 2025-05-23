using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOpen : MonoBehaviour
{
    private Damageable Damageable;

    private void Awake()
    {
        Damageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Menu" || currentScene.buildIndex == 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        if (!Damageable.IsAlive)
        {
            this.gameObject.SetActive(false);
        }
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
