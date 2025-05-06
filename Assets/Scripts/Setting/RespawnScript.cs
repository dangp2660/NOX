using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour
{
    public string sceneName;
    private GameObject player;
    private GameObject currentCheckpoint;
    private string currentCheckPointID;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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
        currentCheckPointID = checkpoint.name;
        Debug.Log("Current checkpoint set to: " + checkpoint.name);
    }

    public void RespawnPlayer()
    {
        if (player != null && currentCheckpoint != null)
        {
            // Add offset to avoid being inside ground or enemies
            player.transform.position = currentCheckpoint.transform.position + Vector3.up * 1f;
            EnemyManager.instance.respawnEnemy(currentCheckPointID);
        }
        else
        {
            Debug.LogWarning("Respawn failed. Reloading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}
