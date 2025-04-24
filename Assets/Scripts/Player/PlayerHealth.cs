using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : Damegeable
{
    private PlayerInput input;
    // Update is called once per frame
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        if(this.getIsAlive())
        {
            input.enabled = false;
        }
    }
}
