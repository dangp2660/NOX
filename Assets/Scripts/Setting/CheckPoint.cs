using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawnScript;
    private bool isPlayerInRange = false;
    private GameObject player;
    [SerializeField] private string checkpointID = "KV1";
    private GameObject PlayerManager;
    public string GetID() => checkpointID;

    private void Start()
    {
        respawnScript = GameObject.FindGameObjectWithTag("Respawn")?.GetComponent<RespawnScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
        PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        if (respawnScript == null)
        {
            Debug.LogError("RespawnScript not found on any object with tag 'Respawn'.");
        }
    }


    private void Update()
    {
        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AudioManager.instance.playSFX(AudioManager.instance.checkPoint);
            respawnScript.SetCheckpoint(this.gameObject);
            Debug.Log("Checkpoint set: " + checkpointID);
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