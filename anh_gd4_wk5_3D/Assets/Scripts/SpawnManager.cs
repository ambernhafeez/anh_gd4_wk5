using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject[] powerupPrefabs;
    private PlayerController playerControllerScript;
    public GameObject mushroom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefabs[0], GenerateSpawnPosition(), powerupPrefabs[0].transform.rotation);
        Instantiate(powerupPrefabs[1], GenerateSpawnPosition(), powerupPrefabs[1].transform.rotation);
        Instantiate(powerupPrefabs[2], GenerateSpawnPosition(), powerupPrefabs[2].transform.rotation);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        // when all enemies are destroyed, spawn next wave and powerups
        if (enemyCount == 0) 
        { 
            SpawnRandomPowerup();
            waveNumber ++; SpawnEnemyWave(waveNumber); 
            
            // spawn another powerup every third round
            if (waveNumber % 3 == 0) { SpawnRandomPowerup(); }
        }

        if (playerControllerScript.hasBouncePowerup)
        {
            Instantiate(mushroom, GenerateSpawnPosition(), mushroom.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition () 
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn) {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyInt = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyInt], GenerateSpawnPosition(), enemyPrefab[enemyInt].transform.rotation);
        }
    }

    void SpawnRandomPowerup()
    {
        int powerupInt = Random.Range(0,powerupPrefabs.Length);
        Instantiate(powerupPrefabs[powerupInt], GenerateSpawnPosition(), powerupPrefabs[powerupInt].transform.rotation);
    }
}
