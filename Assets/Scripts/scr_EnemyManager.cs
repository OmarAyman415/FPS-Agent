using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemyManager : MonoBehaviour
{
    public scr_EnemyController enemyController;
    public Transform[] SpawnPoint;
    public GameObject EnemyPrefab;
    public int round = 0;
    public int enemySpawnAmount = 0;
    public int enemiesKilled = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        scr_EnemyController.OnEnemyKilled += SpawnNewEnemy;
    }

    void SpawnNewEnemy()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(0f, SpawnPoint.Length - 1));

        Instantiate(EnemyPrefab, SpawnPoint[randomNumber].transform.position, SpawnPoint[randomNumber].transform.rotation);
    }
}
