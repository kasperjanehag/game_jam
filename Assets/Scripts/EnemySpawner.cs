using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public DamageReceiver player;
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
        newWaveTimer = 2;
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
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    Enemy npc = enemy.GetComponent<Enemy>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }
    }


    void OnGUI() {
        GUI.Box(new Rect(10, Screen.height - 35, 100, 25), ((int)player.playerHP).ToString() + " HP");

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
