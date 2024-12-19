using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRange = 9;
    public int enemyCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(5);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<Enemy>().Length;
        if (enemyCount == 0) { SpawnEnemyWave(1); }
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
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
