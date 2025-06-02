using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public Transform player;               // Player reference
    public GemPool gemPool;                // Reference to GemPool
    public float gemSpacing = 2f;          // Z-axis spacing between gems
    public float verticalOffset = 1f;      // Y-axis offset
    public float laneWidth = 1f;           // X offset from center (optional)
    public int gemsPerSpawn = 4;           // Number of gems in a row
    public float spawnIntervalZ = 30f;     // Spawn gems every 30 units

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
        float startZ = player.position.z + 10f; // Spawn ahead of player

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
