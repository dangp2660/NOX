using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutSceneManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerSwitch;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        Player.GetComponent<PlayerInput>().enabled = false;
        PlayerSwitch.GetComponent<PlayerSwitch>().enabled = false;
    }

}
