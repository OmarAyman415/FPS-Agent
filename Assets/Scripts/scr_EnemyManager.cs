using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemyManager : MonoBehaviour
{
    public static scr_EnemyManager instance;

    public scr_EnemyController enemyController;
    public Transform[] SpawnPoint;
    public GameObject EnemyPrefab;
    public int round = 0;
    public int enemySpawnAmount = 3;
    public int enemiesKilled = 0;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartWave();
    }


    private void Update()
    {
        if (enemiesKilled >= enemySpawnAmount)
        {
            NextWave();
        }
    }

    void SpawnNewEnemy()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(0f, SpawnPoint.Length - 1));

        Instantiate(EnemyPrefab, SpawnPoint[randomNumber].transform.position, SpawnPoint[randomNumber].transform.rotation);
    }

    void StartWave()
    {
        round = 1;
        enemySpawnAmount = 2;
        enemiesKilled = 0;
        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnNewEnemy();
        }
    }

    void NextWave()
    {
        round++;
        enemySpawnAmount += 2;
        enemiesKilled = 0;

        for (int i =0; i < enemySpawnAmount; i++)
        {
            SpawnNewEnemy();
            new WaitForSeconds(6f);
        }
    }

    public void  IncremenetKilled()
    {
        enemiesKilled++;
    }
}
