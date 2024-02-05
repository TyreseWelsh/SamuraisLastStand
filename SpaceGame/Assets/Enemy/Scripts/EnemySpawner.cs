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
    const int BASE_SPAWN_TIME = 15;
    int spawnTime = 0;
    float waveTimer = 0.0f;
    const int BASE_WAVE_TIME = 40;
    int waveTime = 0;

    int waveNum = 1;
    public int numCurrentEnemies = 0;

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

        Instantiate(enemy, planet.transform.position + (directionToPoint * 30.0f), Quaternion.identity);
        numCurrentEnemies++;
    }

    private void IncreaseWave()
    {
        waveNum++;
        print("Current Wave: " + waveNum);

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
            // ALLOW TO SPAWN ENEMIES WITH STRONGER SHIELD
            enemiesToSpawn = BASE_ENEMIES_TO_SPAWN;
            spawnTime = BASE_SPAWN_TIME;
        }
    }
}
