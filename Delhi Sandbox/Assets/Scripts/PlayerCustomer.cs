using UnityEngine;
using System.Collections;

public class PlayerCustomer : MonoBehaviour {

    private bool carryingCustomer = false, arrived = false;
    private GameObject targetCustomer = null;
    private Rigidbody rb;
    private GameObject customerDestination = null;

    private GameObject[] SpawnPoints;

    public float maxCustPickupSpeed = .25f;
    public Material spawnPointDefault, spawnPointTarget;
    public Transform customerRoot;

    private Customer cust;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entering " + other.name);

        // If entering the range of a customer and one isn't already being targeted, target the customer.
        if (other.tag == "Customer" && targetCustomer == null)
        {
            Debug.Log("Customer spotted.");
            targetCustomer = other.transform.parent.gameObject;
            cust = targetCustomer.GetComponentInParent<Customer>();
            cust.Target();
        }
            
        else if (other.gameObject == customerDestination)
        {
            Debug.Log("Close to destination.");
            arrived = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        //Debug.Log("Exiting " + other.name);

        // If exiting the range of a targeted customer, stop targeting the.
        if (other.tag == "Customer" && other.transform.parent.gameObject == targetCustomer)
        {
            Debug.Log("Leaving potential customer.");
            cust.Pass();
            targetCustomer = null;
            cust = null;
        }


        else if (other == customerDestination)
        {
            Debug.Log("Leaving Destination without dropping customer.");
            arrived = false;
        }
            
    }

    void Update()
    {
        // If not carying a customer and one has been targeted and the care is moving slow enough, pick up the customer.
        if (!carryingCustomer && targetCustomer != null && rb.velocity.magnitude <= maxCustPickupSpeed)
        {
            Debug.Log("Picking up customer.");
            cust.Pickup();
            carryingCustomer = true;

            // Randomly pick a destination to drop off the customer and change it's color.
            int randomIndex = Random.Range(0, SpawnPoints.Length);
            customerDestination = SpawnPoints[randomIndex];
            customerDestination.GetComponent<Renderer>().material = spawnPointTarget;

        }

        // You have arrived at the customer's destination.
        else if (arrived && rb.velocity.magnitude <= maxCustPickupSpeed)
        {
            Debug.Log("Dropping Off Customer");
            customerDestination.GetComponent<Renderer>().material = spawnPointDefault;
            customerDestination = null;
            cust.DropOff();
            targetCustomer = null;
            arrived = false;
            carryingCustomer = false;

        }

    }



}
