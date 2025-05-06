using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawnScript;
    private bool isPlayerInRange = false;
    private GameObject player;
    [SerializeField] private string checkpointID = "KV1";
    public string GetID() => checkpointID;

    private void Start()
    {
        respawnScript = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.playSFX(AudioManager.instance.checkPoint);
            respawnScript.SetCheckpoint(this.gameObject);
            SaveData data = new SaveData();
            Vector2 pos = player.transform.position;
            data.playerX = pos.x;
            data.playerY = pos.y;
            data.currentHealth = player.GetComponent<Damageable>().CurrentHealth;
            data.currentEnergy = player.GetComponent<DarkEnergyManager>().CurrentDarkEnergy;
            data.currentMap = SceneManager.GetActiveScene().name;
            SaveManager.Save(data);
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