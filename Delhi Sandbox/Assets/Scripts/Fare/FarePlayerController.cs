using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FarePlayerController : MonoBehaviour {

	public float speed;
	public float pricePerMile;

	[HideInInspector] public Fare fare;
	[HideInInspector] public FareResponse fareResponse;
	[HideInInspector] public float money;
	[HideInInspector] public float charge;

	float lastPickupTime;
	Vector3 lastPickupPosition;
	Vector3 lastPosition;
	float pathDistance;

	void Awake() 
	{
		money = 0.0f;
		charge = 0.0f;
		lastPickupTime = -Mathf.Infinity;
		lastPosition = transform.position;
		pathDistance = 0.0f;
	}

	void Update ()
	{
		if (null != fare) 
		{
			pathDistance += Vector3.Distance (transform.position, lastPosition);
			lastPosition = transform.position;
			charge = FareTools.roundTwoDecimals (pathDistance * pricePerMile);
		}
	}

	public void PickUp (Customer customer)
	{
		fareResponse = null;
		fare = customer.fare;
		lastPickupTime = Time.time;
		lastPosition = transform.position;
		lastPickupPosition = lastPosition;
		pathDistance = 0;
	}

	public void DropOff()
	{
		float price = pathDistance * pricePerMile;
		float journeyTime = Time.time - lastPickupTime;
		float directDistance = Vector3.Distance (transform.position, lastPickupPosition);
		pathDistance += Vector3.Distance (transform.position, lastPosition);
		fareResponse = fare.GetResponse (directDistance, journeyTime, price);
		money += fareResponse.Payment;
		fare = null;
		// Printing to console - this is visible when running in Unity
		//			print ("directDistance: " + directDistance + "miles\n" +
		//				"pathDistance: " + pathDistance + "miles\n" +
		//				"journeyTime: " + journeyTime + "hrs\n" +
		//				"average speed direct: " + FareTools.getSpeedMph (directDistance, journeyTime) + "mph\n" +
		//				"average speed path: " + FareTools.getSpeedMph (pathDistance, journeyTime) + "mph\n" +
		//				"price: $" + price + "\n" +
		//				"fareResponse.Payment: $" + fareResponse.Payment + "\n" +
		//				"fareResponse.Verbal: \"" + fareResponse.Verbal + "\"");
	}
}
