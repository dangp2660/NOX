using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CheckPoint.cs
public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawnScript;
    private AudioManager audioManager;
    private bool isPlayerInRange = false;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        respawnScript = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            audioManager.playSFX(audioManager.checkPoint);
            respawnScript.SetCheckpoint(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
