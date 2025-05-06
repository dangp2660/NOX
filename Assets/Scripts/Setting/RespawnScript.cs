using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour
{
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
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.transform.position + Vector3.up * 1f;
            AudioManager.instance.playSFX(AudioManager.instance.reSpawn);
            // Add offset to avoid being inside ground or enemies
            EnemyManager.instance.respawnEnemy(currentCheckPointID);
            StartCoroutine(FadeRespawn());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    IEnumerator FadeRespawn()
    {
        SceneController.instance.Transition.SetTrigger(AnimationStringList.End);
        yield return new WaitForSeconds(1f);
        SceneController.instance.Transition.SetTrigger(AnimationStringList.Start);
    }
}
