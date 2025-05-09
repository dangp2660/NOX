using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawnScript;
    private bool isPlayerInRange = false;
    private GameObject player;
    [SerializeField] private string checkpointID = "KV1";
    [SerializeField] private GameManager gameManager;
    public string getID()
    {
        return checkpointID;
    }
    private void Start()
    {
        respawnScript = GameObject.FindGameObjectWithTag("Respawn")?.GetComponent<RespawnScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("Save").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.playSFX(AudioManager.instance.checkPoint);
            respawnScript.SetCheckpoint(this.gameObject);
            gameManager.SaveAtCheckpoint(respawnScript.sceneName, this.transform.position, gameObject.name);
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
