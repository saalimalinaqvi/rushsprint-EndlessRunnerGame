using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs; // Array of track prefabs
    public Transform player; // Player reference
    private List<GameObject> activeTracks = new List<GameObject>(); // List of active tracks
    private float spawnZ = 0f; // Position to spawn the next track
    private int initialTracks = 3; // Initial number of tracks
    private int nextSpawnCount = 2; // Tracks to spawn after reaching mid-point
    private float lastTrackLength = 50f; // Default track length (updated dynamically)

    void Start()
    {
        Debug.Log("TrackManager Started");
        for (int i = 0; i < initialTracks; i++)
        {
            SpawnTrack();
        }
    }

    void Update()
    {
        if (activeTracks.Count > 0)
        {
            GameObject lastTrack = activeTracks[activeTracks.Count - 1];

            // Get last track length dynamically
            float trackLength = GetTrackLength(lastTrack);

            float lastTrackMidPoint = spawnZ - trackLength / 2;
            float lastTrackEnd = spawnZ - trackLength;

            if (player.position.z > lastTrackMidPoint)
            {
                Debug.Log("Player reached mid-point, spawning " + nextSpawnCount + " tracks");
                SpawnMultipleTracks(nextSpawnCount);
                nextSpawnCount = 3; // After 2, spawn 3 next time
            }

            if (player.position.z > lastTrackEnd)
            {
                Debug.Log("Player reached end-point, spawning " + nextSpawnCount + " tracks");
                SpawnMultipleTracks(nextSpawnCount);
                nextSpawnCount = 2; // After 3, spawn 2 next time
                DeleteOldTrack();
            }
        }
    }

    void SpawnMultipleTracks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnTrack();
        }
    }

    void SpawnTrack()
    {
        if (trackPrefabs.Length == 0)
        {
            //Debug.LogError("No track prefabs assigned!");
            return;
        }

        GameObject track = Instantiate(trackPrefabs[Random.Range(0, trackPrefabs.Length)],
                                       new Vector3(0, 0, spawnZ),
                                       Quaternion.identity);

        float trackLength = GetTrackLength(track);
        //Debug.Log("Spawned track at Z: " + spawnZ + " with length: " + trackLength);

        activeTracks.Add(track);
        spawnZ += trackLength; // Move spawn position forward
    }

    void DeleteOldTrack()
    {
        if (activeTracks.Count > 5) // Keep only a few active tracks
        {
            //Debug.Log("Deleting track");
            Destroy(activeTracks[0]);
            activeTracks.RemoveAt(0);
        }
    }

    float GetTrackLength(GameObject track)
    {
        Renderer renderer = track.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.z; // Get the length dynamically
        }
        else
        {
            Debug.LogError("Renderer not found! Using default length.");
            return lastTrackLength; // Fallback length
        }
    }
}












/*using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs; // Array of track prefabs
    public Transform player; // Reference to player
    private List<GameObject> activeTracks = new List<GameObject>(); // List of active tracks
    private float spawnZ = 0f; // Position to spawn the next track
    private int initialTracks = 3; // Initial number of tracks
    private int nextSpawnCount = 2; // Tracks to spawn after mid-point
    private float lastTrackLength = 300f; // Default track length

    void Start()
    {
        Debug.Log("TrackManager Started");
        for (int i = 0; i < initialTracks; i++)
        {
            SpawnTrack();
        }
    }

    void Update()
    {
        if (activeTracks.Count > 0)
        {
            GameObject lastTrack = activeTracks[activeTracks.Count - 1];

            Transform midPoint = lastTrack.transform.Find("MidPoint");
            Transform endPoint = lastTrack.transform.Find("EndPoint");

            if (midPoint != null && player.position.z > midPoint.position.z)
            {
                Debug.Log("Player reached mid-point, spawning " + nextSpawnCount + " tracks");
                SpawnMultipleTracks(nextSpawnCount);
                nextSpawnCount = 3; // Alternate pattern
            }

            if (endPoint != null && player.position.z > endPoint.position.z)
            {
                Debug.Log("Player reached end-point, spawning " + nextSpawnCount + " tracks");
                SpawnMultipleTracks(nextSpawnCount);
                nextSpawnCount = 2; // Alternate pattern
                DeleteOldTrack();
            }
        }
    }

    void SpawnMultipleTracks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnTrack();
        }
    }

    void SpawnTrack()
    {
        if (trackPrefabs.Length == 0)
        {
            Debug.LogError("No track prefabs assigned!");
            return;
        }

        GameObject track = Instantiate(trackPrefabs[Random.Range(0, trackPrefabs.Length)],
                                       new Vector3(0, 0, spawnZ),
                                       Quaternion.identity);

        Debug.Log("Spawned track at Z: " + spawnZ);

        Transform endPoint = track.transform.Find("EndPoint");
        if (endPoint != null)
        {
            lastTrackLength = endPoint.position.z - spawnZ;
            Debug.Log("Track Length: " + lastTrackLength);
        }
        else
        {
            Debug.LogError("EndPoint not found in track prefab!");
        }

        activeTracks.Add(track);
        spawnZ += lastTrackLength; // Move spawn position
    }

    void DeleteOldTrack()
    {
        if (activeTracks.Count > 5) // Keep only a few active tracks
        {
            Debug.Log("Deleting track");
            Destroy(activeTracks[0]);
            activeTracks.RemoveAt(0);
        }
    }
}*/









/*using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs;
    public Transform player;
    private List<GameObject> activeTracks = new List<GameObject>();
    private float spawnZ = 0f;
    private float trackLength = 200f;
    private int numTracksOnScreen = 1;

    void Start()
    {
        for (int i = 0; i < numTracksOnScreen; i++)
        {
            SpawnTrack();
        }
    }

    void Update()
    {
        if (player.position.z - trackLength > (spawnZ - numTracksOnScreen * trackLength))
        {
            SpawnTrack();
            DeleteOldTrack();
        }
    }

    void SpawnTrack()
    {
        GameObject track = Instantiate(trackPrefabs[Random.Range(0, trackPrefabs.Length)], new Vector3(0, 0, spawnZ), Quaternion.identity);
        activeTracks.Add(track);
        spawnZ += trackLength;
    }

    void DeleteOldTrack()
    {
        Destroy(activeTracks[0]);
        activeTracks.RemoveAt(0);
    }
}*/
