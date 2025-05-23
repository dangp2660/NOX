using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EnemyManager instance;
    private List<Enemy> allEnemies = new List<Enemy>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void addEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            allEnemies.Add(enemy);
            Debug.Log(enemy);
        }
    }

    public void respawnEnemy(string checkPointID)
    {
        if (allEnemies.Count == 0)
        {
            Debug.LogWarning("No enemies registered in EnemyManager");
            return;
        }

        foreach (var enemy in allEnemies)
        {
            if (enemy != null && enemy.getCheckPointID() == checkPointID)
            {
                Debug.Log("Respawning enemy at checkpoint: " + checkPointID);
                enemy.respawnEnemy();
            }
        }
    }
    
}
