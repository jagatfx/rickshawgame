using UnityEngine;
using System.Collections;

public class PlayerCustomer : MonoBehaviour {

    public GameObject passengers;
    public GameObject farePlayerControllerObj;

    [HideInInspector] public FarePlayerController farePlayerController;

    // The max speed you must be traveling in order to pick up or drop off a customer.
    public float maxCustPickupSpeed = 0.25f;

    // Indicates if you are currently carrying a customer and if
    // you are in range of their destination.
    bool carryingCustomer = false;
    bool arrived = false;

    // Customer currently being targeted. This is currently the
    // first customer you come into pickup range of. TODO:
    // change to closest customer.
    GameObject targetCustomer = null;
    // References to scripts related to your current customer.
    Customer cust;
    SpawnPoint dest;
    // Referece to the destination of your current customer.
    GameObject customerDestination = null;

    Compass compass;
    RickshawAIControl aiController;

    // Reference to rigid body of car. Used for checking current speed.
    Rigidbody rb;

    #region MonoBehavior Events

    void Awake()
    {
        // Initialize References
        rb = GetComponent<Rigidbody>();
        farePlayerController = farePlayerControllerObj.GetComponent<FarePlayerController> ();
        compass = GetComponent<Compass>();
        aiController = GetComponentInParent<RickshawAIControl>();
    }

    void OnTriggerStay(Collider other)
    {
        // If dropping off a customer at their destination and there is a customer waiting there
        // then don't require leaving and reentering the range of the new customer to be able to
        // pick them up. TODO: Consider not allowing destinations in range of awaiting customers.
        // Would allow the elimination of the check on stay.
        if (other.tag == "Customer" && targetCustomer == null)
            TargetCustomer(other.transform.parent.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // If approaching the customers target destination...
        if (other.gameObject == customerDestination)
            CloseToDestination();

        // TODO: commented out because now being handled in OnTriggerStay. If eliminating customers
        // at dropoff, uncomment this and remove OnTriggerStay.
        // If entering the range of a customer and one is not already being targeted.
        //else if (other.tag == "Customer" && targetCustomer == null)
        //    TargetCustomer(other.transform.parent.gameObject);

    }

    void OnTriggerExit(Collider other)
    {

        // If exiting the range of a targeted customer, stop targeting the.
        if (other.tag == "Customer" && other.transform.parent.gameObject == targetCustomer)
            PassCustomer();
        else if (other.gameObject == customerDestination)
            MissedDestination();

    }

    void UpdateForTarget()
    {
        // Pick the compass target. If not targeting customer, find the closest one.
        // If targeting customer, point at it. If carrying a customer, point to the destination.
        GameObject currentTarget = null;
        if (!carryingCustomer && targetCustomer == null)
        {
            currentTarget = ClosestCustomer();
        }
        else if (!carryingCustomer && targetCustomer != null)
        {
            currentTarget = targetCustomer;
        }
        else if (carryingCustomer)
        {
            currentTarget = customerDestination;
        }

        if (currentTarget)
        {
            if (compass)
            {
                compass.SetTarget (currentTarget);
            }
            if (aiController)
            {
                aiController.SetTarget (currentTarget.transform);
            }
        }
    }

    void Update()
    {
        UpdateForTarget ();

        if (!carryingCustomer && targetCustomer != null && 
            targetCustomer.GetComponent<Customer>().IsSeekingRide() &&
            rb.velocity.magnitude <= maxCustPickupSpeed)
        {
            // If not carrying a customer and one has been targeted and 
            // the car is moving slow enough, pick up the customer.
            PickupCustomer();
        }
        else if (arrived && rb.velocity.magnitude <= maxCustPickupSpeed)
        {
            // You have arrived at the customer's destination.
            DropOffCustomer();
        }
    }

    #endregion

    #region Player/Customer/Destination Interactions
    void TargetCustomer(GameObject customer)
    {
        Debug.Log("Player: Customer spotted.");
        targetCustomer = customer;
        cust = targetCustomer.GetComponentInParent<Customer>();
        cust.Hail();
    }

    void PassCustomer()
    {
        Debug.Log("Player: Leaving potential customer.");
        cust.Miss();
        targetCustomer = null;
        cust = null;
    }

    void PickupCustomer()
    {
        Debug.Log("Player: Picking up customer.");
        farePlayerController.PickUp (cust);
        cust.Pickup(passengers);
        carryingCustomer = true;

        customerDestination = cust.destination;
        dest = customerDestination.GetComponent<SpawnPoint>();
        dest.Activate();
    }

    void CloseToDestination()
    {
        Debug.Log("Player: Close to destination.");
        arrived = true;
        dest.Enter();
    }

    void MissedDestination()
    {
        Debug.Log("Player: Leaving Destination without dropping customer.");
        arrived = false;
        dest.Exit();
    }

    void DropOffCustomer()
    {
        Debug.Log("Player: Dropping Off Customer");
        farePlayerController.DropOff ();
        customerDestination = null;
        cust.DropOff();
        targetCustomer = null;
        arrived = false;
        carryingCustomer = false;
        dest.Deactivate();
    }

    void EjectCustomer()
    {

    }

    GameObject ClosestCustomer()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");

        if (null != customers && customers.Length > 0)
        {
            int closest = 0;
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < customers.Length; i++) {
                Customer customer = customers [i].GetComponent<Customer>();
                if (!customer || !customer.IsSeekingRide())
                {
                    continue;
                }
                float dist = Vector3.Distance (customers[i].transform.position, transform.position);
                if (dist < closestDist)
                {
                    closest = i;
                    closestDist = dist;
                }
            }
            return customers[closest];
        }
        return null;
    }

    #endregion

}
