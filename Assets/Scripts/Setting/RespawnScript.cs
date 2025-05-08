using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour
{
    public string sceneName;
    private GameObject player;
    private GameObject currentCheckpoint;
    private string currentCheckPointID;
    private Damageable health;
    private DarkEnergyManager darkEnergy;
    private PlayerSwitch playerSwitch;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSwitch = GameObject.FindAnyObjectByType<PlayerSwitch>();
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
        //Find again Player if player transform
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        player = newPlayer;
        currentCheckpoint = checkpoint;
        currentCheckPointID = checkpoint.name;
        Debug.Log("Current checkpoint set to: " + checkpoint.name + ", " + checkpoint);
        SaveGameManager.SaveGame(player.transform.position,
            player.GetComponent<Damageable>().CurrentHealth,
            player.GetComponent<Damageable>().getMaxHealth(),
            DarkEnergyManager.instance.CurrentDarkEnergy,
            DarkEnergyManager.instance.MaxDarkEnergy,
            sceneName,
            currentCheckpoint.name,
            PlayerSwitch.Instance.isDefault);
    }

    public void RespawnPlayer()
    {
        if (currentCheckpoint != null)
        {

            Debug.Log(currentCheckpoint.name);
            player.transform.position = currentCheckpoint.transform.position;
            AudioManager.instance.playSFX(AudioManager.instance.reSpawn);
            // Add offset to avoid being inside ground or enemies
            EnemyManager.instance.respawnEnemy(currentCheckPointID);
            StartCoroutine(FadeRespawn());
            SaveGameManager.SaveGame(player.transform.position,
                player.GetComponent<Damageable>().getMaxHealth(), 
                player.GetComponent<Damageable>().getMaxHealth(),
                DarkEnergyManager.instance.MaxDarkEnergy,
                DarkEnergyManager.instance.MaxDarkEnergy,
                sceneName, currentCheckpoint.name, PlayerSwitch.Instance.isDefault);    
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void LoadFromData(SaveData data)
    {
        if(data != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(data.playerX, data.playerY, 0f);
            player.GetComponent<Damageable>().CurrentHealth = data.currentHealth;
            player.GetComponent<DarkEnergyManager>().CurrentDarkEnergy = data.currentDarkEnergy;
            currentCheckpoint = GameObject.Find(data.currentCheckPointName);
            playerSwitch.isDefault = data.isDefault;
        }
    }
    IEnumerator FadeRespawn()
    {
        SceneController.instance.Transition.SetTrigger(AnimationStringList.End);
        yield return new WaitForSeconds(1f);
        SceneController.instance.Transition.SetTrigger(AnimationStringList.Start);
    }
}