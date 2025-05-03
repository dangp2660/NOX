using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CheckPoint.cs
public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawnScript;

    private void Start()
    {
        respawnScript = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            respawnScript.SetCheckpoint(this.gameObject);
        }
    }
}

