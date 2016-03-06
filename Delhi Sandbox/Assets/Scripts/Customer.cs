using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

    public Material happyMaterial;

    Collider taxiCollider;
    MeshRenderer status;

	void Awake() {
        taxiCollider = GetComponent<Collider>();
        status = GetComponent<MeshRenderer>();
	}
	
	public void Pickup()
    {
        Debug.Log("I am being picked up!");
        transform.parent = GameObject.Find("PlayerCar").transform;
        status.material = happyMaterial;
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        taxiCollider.enabled = false;
    }

}
