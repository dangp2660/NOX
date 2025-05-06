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
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void addEnemy(Enemy enemy)
    {
        allEnemies.Add(enemy);
    }

    public void respawnEnemy(string checkPointID)
    {
        foreach(var enemy in allEnemies)
        {
            if(enemy != null && enemy.getCheckPointID() == checkPointID)
            {
                enemy.respawnEnemy();
            }
        }
    }
}
