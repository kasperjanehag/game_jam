using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] m_enemyPrefabs;
    public Transform playerTransform;
    public float spawnInterval = 2; //Spawn new enemy each n seconds
    public int enemiesPerWave = 5; //How many enemies per wave
    public Transform[] spawnPoints;

    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;
    //How many enemies we already eliminated in the current wave
    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Wait 10 seconds for new wave to start
        newWaveTimer = GameManager.Instance.Config.WaveInterval;
        waitingForWave = true;       
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForWave)
        {
            if(newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                //Initialize new wave
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
        }
        else
        {
            if(Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;

                //Spawn enemy 
                if(totalEnemiesSpawned < enemiesToEliminate)
                {

                    var enemy = SpawnRandomEnemy();
                    Enemy npc = enemy.GetComponent<Enemy>();
                    npc.playerTransform = playerTransform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }
    }

    private GameObject SpawnRandomEnemy()
    {
        var index = (int)Random.Range(0f, m_enemyPrefabs.Length);
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        GameObject enemy = Instantiate(m_enemyPrefabs[index], randomPoint.position, Quaternion.identity);
        return enemy;
    }

    public void EnemyEliminated(Enemy enemy)
    {
        enemiesEliminated++;

        if(enemiesToEliminate - enemiesEliminated <= 0)
        {
            //Start next wave
            newWaveTimer = 10;
            waitingForWave = true;
            waveNumber++;
        }
    }
}
