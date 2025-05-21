using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpen : MonoBehaviour
{
    private Damageable Damageable;
    private void Awake()
    {
        Damageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!Damageable.IsAlive)
        {
            this.gameObject.SetActive(false);
        }
    }
}
