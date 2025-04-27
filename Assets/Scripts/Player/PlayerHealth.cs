using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    private Damageable Damegeable;
    private Animator  animator;
    // Update is called once per frame
    private void Awake()
    {
        animator = GetComponent<Animator>();
        Damegeable = GetComponent<Damageable>();
    }
    public bool isAlive()
    {
        return Damegeable.IsAlive;
    }
    private void Update()
    {
        if (!isAlive())
        {
            animator.SetBool(AnimationStringList.isAlive, false);
        }
    }
}
