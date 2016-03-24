using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public bool populated = false;
    public int id;

    Renderer rend;
    Collider coll;

    #region MonoBehavior Events
    void Awake()
    {
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
    }
    #endregion

    #region Spawnpoint Actions
    /// <summary>
    /// Player has picked up a customer and their destination must be made visible.
    /// </summary>
    public void Activate()
    {
        rend.material = SpawnManager.SpawnPointTargetMat();
        coll.enabled = true;
        rend.enabled = true;
    }

    /// <summary>
    /// Player has entered the SpawnPoint
    /// </summary>
    public void Enter()
    {
        rend.material = SpawnManager.SpawnPointInRangeMat();
    }

    /// <summary>
    /// Player has exited the Spawnpoint
    /// </summary>
    public void Exit()
    {
        rend.material = SpawnManager.SpawnPointTargetMat();
    }

    /// <summary>
    /// Player has dropped off customer and it can be made invisible again.
    /// </summary>
    public void Deactivate()
    {
        rend.material = SpawnManager.SpawnPointDefaultMat();
        coll.enabled = false;
        if (SpawnManager.HideSpawnPoints())
            rend.enabled = false;
    }
    #endregion
}
