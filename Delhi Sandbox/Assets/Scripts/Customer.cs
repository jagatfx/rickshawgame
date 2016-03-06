using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

    public Material happyCustMat;
    public Material inRangeMat;

    MeshRenderer custRend;
    GameObject range;
    Transform customerRoot;
    Renderer rangeRend;
    Material outRange;

    void Awake() {
        customerRoot = transform;
        custRend = GetComponent<MeshRenderer>();
        range = transform.GetChild(0).gameObject;
        rangeRend = range.GetComponent<Renderer>();
        outRange = rangeRend.material;
	}
	
    public void Target()
    {
        Debug.Log("Taxi!");
        rangeRend.material = inRangeMat;
    }

    public void Pass()
    {
        Debug.Log("Hey! Come back!");
        rangeRend.material = outRange;
    }

	public void Pickup()
    {
        // When picking up, set the parent to the player's passenger area.
        // Change the color of the customer. Disable the range sphere.
        Debug.Log("I am being picked up!");
        transform.parent = GameObject.Find("Passengers").transform;
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        custRend.material = happyCustMat;
        range.SetActive(false);
    }

    public void DropOff()
    {
        // When cdropping off customer, transfer from passenger area to the
        // customer root in the heirarchy, and then disable them.
        Debug.Log("Thanks for the ride!");
        transform.parent = customerRoot;
        gameObject.SetActive(false);
    }

}
