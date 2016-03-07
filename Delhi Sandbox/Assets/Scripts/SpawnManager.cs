using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject customerPrefab;
    public GameObject customerParent;

    public Material spawnPointDefaultMat, spawnPointTargetMat, spawnPointInRangeMat;
    public bool hideSpawnPoints = true;

    /// <summary>
    /// Singleton meant to manage spawns.
    /// </summary>
    public static SpawnManager instance = null;

    /// <summary>
    /// An array of all spawnpoints under the the game object that owns this script.
    /// Maybe convert to list in the future and allow user to add spawn points manually
    /// from around the scene which may be nested under other game objects.
    /// </summary>
    private GameObject[] spawnPoints;
    private List<int> openSpawnPoints = new List<int>();

    public int maxCustomers = 3;
    private int availableCustomers = 0;

    #region MonoBehavior Events
    void Awake()
    {
        // Init singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Destroying extra spawnmanager instance.");
            Destroy(gameObject);
        }

        // Find all spawnpoints on the map
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        // Keep track of which spawn points are empty.
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnPoint sp = spawnPoints[i].GetComponent<SpawnPoint>();
            sp.populated = false;
            sp.id = i;
            openSpawnPoints.Add(i);
        }

    }

    void Update()
    {
        // Test, spawn passenger at first spawnp point.
        for (int i = availableCustomers; i < maxCustomers; i++)
        {
            Spawn();
        }

    }
    #endregion

    #region SpawnManager Methods
    public void Spawn()
    {
        int id = Random.Range(0, openSpawnPoints.Count);
        Spawn(id);
    }

    /// <summary>
    /// Initiates spawn at a given point. If a negative value is provided, then
    /// a point will be selected at random.
    /// </summary>
    public void Spawn(int id)
    {

        if (id > openSpawnPoints.Count)
        {
            Debug.Log("No more free spawn points.");
            return;
        }

        if (id > spawnPoints.Length)
        {
            Debug.Log(id + " is beyond the max spawn point. Value must be <= " + spawnPoints.Length);
            return;
        }

        if (!openSpawnPoints.Contains(id))
        {
            Debug.Log("Spawn Point " + id + " is already populated.");
            return;
        }

        // Flag the spawn point as popualted and remove it from the free list.
        Debug.Log("Spawning at " + spawnPoints[id].transform.name);
        spawnPoints[id].GetComponent<SpawnPoint>().populated = true;
        Vector3 newPassengerPos = spawnPoints[id].transform.position;
        Quaternion newPassengerRot = spawnPoints[id].transform.rotation;

        GameObject newPassenger = Instantiate(customerPrefab, newPassengerPos, newPassengerRot) as GameObject;
        Customer cust = newPassenger.GetComponent<Customer>();

        newPassenger.transform.parent = customerParent.transform;
        cust.Init(spawnPoints[id], GetDestination(id));
        openSpawnPoints.Remove(id);
        availableCustomers++;
    }

    /// <summary>
    /// Initaites a pickup at a given point.
    /// </summary>
    public static void RemoveCustomer()
    {
        instance.availableCustomers--;
    }

    /// <summary>
    /// Pick a random destination that is not the spawn point. Makes 10 attempts
    /// before falling back on the origin as the destination.
    /// </summary>
    /// <param name="spawnPointId">ID of where the pickup is occuring.</param>
    /// <returns></returns>
    GameObject GetDestination(int spawnPointId)
    {
        int i = spawnPointId;
        for (int cntr = 0; cntr < 10; cntr++)
        {
            i = Random.Range(0, spawnPoints.Length);
            if (i != spawnPointId)
                break;
        }
        return spawnPoints[i];
    }
    #endregion

    #region Static Methods
    public static Material SpawnPointDefaultMat()
    {
        return instance.spawnPointDefaultMat;
    }

    public static Material SpawnPointTargetMat()
    {
        return instance.spawnPointTargetMat;
    }

    public static Material SpawnPointInRangeMat()
    {
        return instance.spawnPointInRangeMat;
    }

    public static bool HideSpawnPoints()
    {
        return instance.hideSpawnPoints;
    }

    public static void FreeSpawnPoint(int id)
    {
        instance.openSpawnPoints.Add(id);
    }
    #endregion
}
