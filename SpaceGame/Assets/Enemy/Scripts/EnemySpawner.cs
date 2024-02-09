using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject planet;
    [SerializeField] GameObject enemy;
    public bool canSpawn = true;


    const int BASE_ENEMIES_TO_SPAWN = 1;
    int enemiesToSpawn = 0;
    float spawnTimer = 0.0f;
    const int BASE_SPAWN_TIME = 6;
    int spawnTime = 0;
    float waveTimer = 0.0f;
    const int BASE_WAVE_TIME = 32;
    int waveTime = 0;

    int waveNum = 1;
    public int numCurrentEnemies = 0;
    int enemyStage = 1;

    float firstSpawnTimer = 0;
    bool firstSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = BASE_ENEMIES_TO_SPAWN;
        spawnTime = BASE_SPAWN_TIME;
        waveTime = BASE_WAVE_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            firstSpawnTimer += Time.deltaTime;

            if (!firstSpawned)
            {
                if (firstSpawnTimer > 8)
                {
                    spawnTimer = 0.0f;
                    waveTimer = 0.0f;

                    SpawnEnemy();
                    firstSpawned = true;
                }
            }

            spawnTimer += Time.deltaTime;
            waveTimer += Time.deltaTime;

            if (spawnTimer >= spawnTime)
            {
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    if (numCurrentEnemies <= 4)
                    {
                        SpawnEnemy();
                    }
                }
                spawnTimer = 0.0f;
            }

            if (waveTimer >= waveTime)
            {
                IncreaseWave();
                waveTimer = 0.0f;
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 directionToPoint = Random.onUnitSphere;

        GameObject spawnedEnemy = Instantiate(enemy, planet.transform.position + (directionToPoint * 50.0f), Quaternion.identity);
        spawnedEnemy.GetComponent<BasicEnemy>()?.shield.GetComponent<EnemyShield>()?.SetStage(enemyStage);
        numCurrentEnemies++;
    }

    private void IncreaseWave()
    {
        waveNum++;

        if (waveNum == 2)
        {
            spawnTime--;
        }
        if(waveNum % 3 == 0)
        {
            enemiesToSpawn++;
            spawnTime += 3;
        }
        if (waveNum % 5 == 0)
        {
            if (enemyStage < 5)
            {
                enemyStage++;
            }
            enemiesToSpawn = BASE_ENEMIES_TO_SPAWN;
            spawnTime = BASE_SPAWN_TIME;
        }
    }
}
