using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject passengerPrefab;
    public GameObject passengerParent;

    /// <summary>
    /// Singleton meant to manage spawns.
    /// </summary>
    public static SpawnManager instance = null;

    /// <summary>
    /// An array of all spawnpoints under the the game object that owns this script.
    /// Maybe convert to list in the future and allow user to add spawn points manually
    /// from around the scene which may be nested under other game objects.
    /// </summary>
    private SpawnPoint[] spawnPoints;

    /// <summary>
    /// List of array indicies of spawnpoints which are not populated.
    /// </summary>
    private List<int> openPoints = new List<int>();

    void Awake()
    {
        // Init singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // Get all spawn points in children of SpawnPointManager.
        // For each of those points, find if they're populated,
        // and assign an id to them so game can tell the manager
        // where things are being picked up from.
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        for (int i=0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].id = i;
            if (!spawnPoints[i].populated)
                openPoints.Add(i);
        }

    }

    void Start()
    {
        // Test, spawn passenger at first spawnp point.
        Spawn(0);
    }

    /// <summary>
    /// Initiates spawn at a given point. If a negative value is provided, then
    /// a point will be selected at random.
    /// </summary>
    public void Spawn(int id = -1)
    {
        int i = id;

        if (openPoints.Count == 0)
        {
            Debug.Log("No Free Spawn Points");
            return;
        }

        if (id > openPoints.Count)
        {
            Debug.Log(id + " is beyond the max spawn point. Value must be <= " + openPoints.Count);
            return;
        }
        else if (id >= 0)
        {
            if (!openPoints.Contains(id))
            {
                Debug.Log("Spawn Point " + id + " is already populated.");
                return;
            }
        }
        else
            i = openPoints[Random.Range(0, openPoints.Count)];

        // Flag the spawn point as popualted and remove it from the free list.
        Debug.Log("Spawning at " + spawnPoints[i].transform.name);
        spawnPoints[i].populated = true;
        Vector3 newPassengerPos = spawnPoints[i].transform.position;
        Quaternion newPassengerRot = spawnPoints[i].transform.rotation;
        GameObject newPassenger = Instantiate(passengerPrefab, newPassengerPos, newPassengerRot) as GameObject;
        newPassenger.transform.parent = passengerParent.transform;
        openPoints.Remove(i);
    }

    /// <summary>
    /// Initaites a pickup at a given point.
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        if (id > spawnPoints.Length)
        {
            Debug.Log(id + " is beyond the max spawn point. Value must be <= " + openPoints.Count);
            return;
        }

        else if (!spawnPoints[id].populated)
        {
            Debug.Log("Spawn Point " + id + " is not populated.");
            return;
        }

        // Flag the spawn point as free and add it to the free list.
        Debug.Log("Picking up at " + spawnPoints[id].transform.name);
        openPoints.Add(id);
        spawnPoints[id].populated = false;
    }

}
