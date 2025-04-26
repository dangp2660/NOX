using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    private Damageable Damegeable;
    // Update is called once per frame
    private void Awake()
    {
        Damegeable = GetComponent<Damageable>();
    }
    public bool isAlive()
    {
        return Damegeable.IsAlive;
    }
}
