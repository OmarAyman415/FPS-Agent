using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemyManager : MonoBehaviour
{
    public scr_EnemyController enemyController;
    public Transform[] SpawnPoint;
    public GameObject EnemyPrefab;
    public int round = 0;
    public int enemiesSpawnAmount = 0;
    public int enemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        StartWave();
        enemiesKilled = enemyController.enemiesKilled;
    }

    private void Update()
    {
        NextWave();
    }

    public void Initialise(scr_EnemyController EnemyController)
    {
        enemyController = EnemyController;
    }

    void SpawnNewEnemy()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(0f, SpawnPoint.Length - 1));

        Instantiate(EnemyPrefab, SpawnPoint[randomNumber].transform.position, SpawnPoint[randomNumber].transform.rotation);
    }

    void StartWave()
    {
        round = 1;
        enemiesSpawnAmount = 2;
        enemyController.enemiesKilled = 0;

        for (int i = 0; i < enemiesSpawnAmount; i++)
        {
            SpawnNewEnemy();
        }
    }

    void NextWave()
    {
        if (enemyController.enemiesKilled >= enemiesSpawnAmount)
        {
            round++;
            enemiesSpawnAmount += 2;
            enemyController.enemiesKilled = 0;

            for (int i = 0; i < enemiesSpawnAmount; i++)
            {
                SpawnNewEnemy();
            }
        }
    }
}
