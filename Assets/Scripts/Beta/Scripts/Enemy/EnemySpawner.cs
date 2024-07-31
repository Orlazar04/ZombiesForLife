// Alpha Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int enemyCount;
    public float spawnStartTime = 3;
    public float spawnPauseTime = 5;

    public GameObject enemyPrefabT1;
    public GameObject enemyPrefabT2;
    public GameObject enemyPrefabT3;

    private int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize difficulty level
        difficulty = LevelManager.levelDifficulty;

        enemyCount = 0;

        InvokeRepeating("SpawnEnemies", spawnStartTime, spawnPauseTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn random enemy at random location
    void SpawnEnemies()
    {
        // While the level is not over
        if(LevelManager.IsLevelActive())
        {
            var spawnChance = Random.Range(0, 150 / difficulty);

            if(spawnChance < 50 && enemyCount < (3 * difficulty))
            {
                enemyCount++;

                // Initialize a random spawn position near spawner
                Vector3 enemyPosition = transform.position;

                // Initialize enemy type
                GameObject enemy = enemyPrefabT1;
                int spawnType = Random.Range(0, 100);

                if(spawnType < 20)
                {
                    enemy = enemyPrefabT3;
                }
                else if(spawnType < 50)
                {
                    enemy = enemyPrefabT2;
                }

                // Spawn enemy
                GameObject spawnedEnemy = Instantiate(enemy, enemyPosition, transform.rotation) as GameObject;
                spawnedEnemy.transform.parent = gameObject.transform.parent; 
            }
        }
    }
}
