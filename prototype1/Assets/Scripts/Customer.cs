﻿using UnityEngine;

public class Customer : MonoBehaviour
{

    public Material happyCustMat;
    public Material inRangeMat;
    public Fare fare;
    public GameObject range;
    public GameObject spawnPoint;
    public GameObject destination;
    public int avatarId;

    Transform customerRoot;
    MeshRenderer custRend;
    Renderer rangeRend;
    Material outRange;
    Rigidbody rb;

    private bool isSeekingRide;

    #region MonoBehavior Events

    void Awake ()
    {
        customerRoot = transform;
        custRend = GetComponent<MeshRenderer> ();
        rangeRend = range.GetComponent<Renderer> ();
        outRange = rangeRend.material;
        rb = GetComponent<Rigidbody>();
        if (Random.Range (0.0f, 1.0f) > 0.5f) {
            fare = new RichFare ();
        }
        else
        {
            fare = new PoorFare ();
        }
        avatarId = CustomerAvatars.RandomId();
        isSeekingRide = true;
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
    public void Init (GameObject custSpawnPoint, GameObject custDestination)
    {
        spawnPoint = custSpawnPoint;
        destination = custDestination;
    }

    /// <summary>
    /// Changes to customer when a taxi is in range.
    /// </summary>
    public void Hail ()
    {
        Debug.Log ("Customer: *Whistle* Taxi!");
        rangeRend.material = inRangeMat;
        CustomerAvatars.Play (avatarId, CustAudioTypes.Hail);
    }

    /// <summary>
    /// Changes to customer when a taxi leaves range.
    /// </summary>
    public void Miss ()
    {
        Debug.Log ("Customer: Hey! Come back!");
        rangeRend.material = outRange;
        CustomerAvatars.Play (avatarId, CustAudioTypes.Miss);
    }

    /// <summary>
    /// Changes to customer when being picked up by player.
    /// </summary>
    public void Pickup (Transform passengerTransform)
    {
        isSeekingRide = false;
        // When picking up, set the parent to the player's passenger area,
        // disable their range sphere.
        Debug.Log("Customer: Take me to " + destination.transform.name + " please.");

        // Turn off the rigid body collision stuff. Rotate them upright
        // and place them in the passenger section.
        rb.isKinematic = true;
        rb.detectCollisions = false;
        transform.parent = passengerTransform;    
        transform.rotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 0, 0);
        range.SetActive(false);
        custRend.material = happyCustMat;
        SpawnManager.FreeSpawnPoint(spawnPoint.GetComponent<SpawnPoint>().id);
    }

    /// <summary>
    /// Changes to customer when their destination has been reached.
    /// </summary>
    public void DropOff ()
    {
        // When dropping off customer, transfer from passenger area to the
        // customer root in the hierarchy, and then disable them.
        Debug.Log ("Customer: Thanks for the ride!");
        SpawnManager.FreeSpawnPoint (destination.GetComponent<SpawnPoint> ().id);
        CustomerAvatars.Play (avatarId, CustAudioTypes.HappyDropoff);
        Reset ();
    }

    public void Reset()
    {
        SpawnManager.RemoveCustomer ();
        transform.parent = customerRoot;
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public bool IsSeekingRide()
    {
        return isSeekingRide;
    }

    #endregion

}
