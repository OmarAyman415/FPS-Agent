using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemyManager : MonoBehaviour
{
    private int waveNumber = 0;
    private int enemySpawnAmount = 0;
    public Transform[] SpawnPoint;
    public GameObject EnemyPrefab;




    // Start is called before the first frame update
    void Start()
    {
        SpawnNewEnemy();
    }

    void OnEnable()
    {
        scr_EnemyController.OnEnemyKilled += SpawnNewEnemy;
    }

    void SpawnNewEnemy()
    {

        int randomNumber = Mathf.RoundToInt(Random.Range(0f, SpawnPoint.Length - 1));

        Instantiate(EnemyPrefab, SpawnPoint[randomNumber].transform.position, Quaternion.identity);
    }

    private void StartWave()
    {

    }

    private void NextWave()
    {

    }
}
