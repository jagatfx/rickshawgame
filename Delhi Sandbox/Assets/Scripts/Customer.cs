﻿using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

    public Material happyCustMat, inRangeMat;

    Transform customerRoot;
    MeshRenderer custRend;
    GameObject range;
    Renderer rangeRend;
    Material outRange;

    public GameObject spawnPoint;
    public GameObject destination;

    #region MonoBehavior Events
    void Awake() {
        customerRoot = transform;
        custRend = GetComponent<MeshRenderer>();
        range = transform.GetChild(0).gameObject;
        rangeRend = range.GetComponent<Renderer>();
        outRange = rangeRend.material;
	}
    #endregion

    # region Customer Methods
    /// <summary>
    /// Sets the origin and destination of the customer. Not currently in
    /// awake because the SpawnManager handles setting the destination
    /// currently.
    /// </summary>
    /// <param name="custSpawnPoint">Origin</param>
    /// <param name="custDestination">Destination</param>
    public void Init(GameObject custSpawnPoint, GameObject custDestination)
    {
        spawnPoint = custSpawnPoint;
        destination = custDestination;
    }

    /// <summary>
    /// Changes to customer when a taxi is in range.
    /// </summary>
    public void Hail()
    {
        Debug.Log("Customer: *Whistle* Taxi!");
        rangeRend.material = inRangeMat;
    }

    /// <summary>
    /// Changes to customer when a taxi leaves range.
    /// </summary>
    public void Miss()
    {
        Debug.Log("Customer: Hey! Come back!");
        rangeRend.material = outRange;
    }

    /// <summary>
    /// Changes to customer when being picked up by player.
    /// </summary>
    public void Pickup()
    {
        // When picking up, set the parent to the player's passenger area,
        // disable their range sphere.
        Debug.Log("Customer: Take me to " + destination.transform.name + " please.");
        transform.parent = GameObject.Find("Passengers").transform;
        transform.localPosition = new Vector3(0, 0, 0);
        range.SetActive(false);
        custRend.material = happyCustMat;
        SpawnManager.FreeSpawnPoint(spawnPoint.GetComponent<SpawnPoint>().id);
    }

    /// <summary>
    /// Changes to customer when their destination has been reached.
    /// </summary>
    public void DropOff()
    {
        // When dropping off customer, transfer from passenger area to the
        // customer root in the hierarchy, and then disable them.
        Debug.Log("Customer: Thanks for the ride!");
        transform.parent = customerRoot;
        SpawnManager.RemoveCustomer();
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
    #endregion

}
