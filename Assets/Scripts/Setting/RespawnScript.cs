using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// RespawnScript.cs
public class RespawnScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameObject currentCheckpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentCheckpoint != null)
        {
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log("current Point" + currentCheckpoint);

    }

    public void RespawnPlayer()
    {
        if (player != null && currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.transform.position;
        }
    }
}

