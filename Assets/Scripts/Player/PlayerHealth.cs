using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    private Damageable Damegeable;
    private PlayerInput input;
    // Update is called once per frame
    private void Awake()
    {
        Damegeable = GetComponent<Damageable>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        if(!Damegeable.IsAlive)
        {
            input.enabled = false;
            Debug.Log(input.enabled);
        }
    }
    public bool isAlive()
    {
        return Damegeable.IsAlive;
    }
}
