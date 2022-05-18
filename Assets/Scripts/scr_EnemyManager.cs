using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_EnemyManager : MonoBehaviour
{
    public static scr_EnemyManager instance;

    public GameObject pauseScreen;
    public GameObject endScreen;
    public TextMeshProUGUI roundNum;
    public scr_EnemyController enemyController;
    public Transform[] SpawnPoint;
    public GameObject EnemyPrefab;
    public GameObject pauseMenu;
    public int round = 0;
    public int enemySpawnAmount = 3;
    public int enemiesKilled = 0;
    [HideInInspector]
    public bool isPaused;

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
            roundNum.text = "Round: " + round.ToString();
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

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        Invoke("LoadMainMenuScene", .4f);
    }

    void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
