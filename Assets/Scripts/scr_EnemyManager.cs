using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EnemyManager : MonoBehaviour
{
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
        Instantiate(EnemyPrefab, SpawnPoint[0].transform.position, Quaternion.identity);
    }
}
