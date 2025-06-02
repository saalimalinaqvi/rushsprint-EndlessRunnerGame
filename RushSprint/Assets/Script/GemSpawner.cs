using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;               // Player reference
    [SerializeField] private GemPool gemPool;                // Reference to GemPool
    [SerializeField] private  float gemSpacing = 2f;          // Z-axis spacing between gems
    [SerializeField] private float verticalOffset = 1f;      // Y-axis offset
    [SerializeField] private float laneWidth = 1f;           // X offset from center (optional)
    [SerializeField] private int gemsPerSpawn = 4;           // Number of gems in a row
    [SerializeField] private float spawnIntervalZ = 30f;     // Spawn gems every 30 units

    private float lastSpawnZ = Mathf.NegativeInfinity;

    void Update()
    {
        if (player.position.z >= lastSpawnZ + spawnIntervalZ)
        {
            SpawnGemsAheadOfPlayer();
            lastSpawnZ = player.position.z;
        }
    }

    private void SpawnGemsAheadOfPlayer()
    {
        float startZ = player.position.z + 10f; // Spawn ahead of player position

        for (int i = 0; i < gemsPerSpawn; i++)
        {
            GameObject gem = gemPool.GetPooledGem();
            if (gem != null)
            {
                Vector3 spawnPos = new Vector3(
                   player.position.x,player.position.y
                    + verticalOffset,startZ + i * gemSpacing );

                gem.transform.position = spawnPos;
                gem.transform.rotation = gemPool.gemPrefab.transform.rotation;
                gem.SetActive(true);
            }
        }
    }
}
