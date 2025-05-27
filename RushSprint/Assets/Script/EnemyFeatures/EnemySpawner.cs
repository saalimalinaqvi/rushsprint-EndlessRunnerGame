using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 30f;
    public Transform player;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isSpawning = true;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 5f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (!isSpawning || GameManager.instance == null || GameManager.instance.IsGameOver() || player == null)
            return;

        Vector3 spawnPos = new Vector3(0, 0, player.position.z + 50f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }

    public void StopAndClearEnemies()
    {
        isSpawning = false;

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        spawnedEnemies.Clear();
    }
}