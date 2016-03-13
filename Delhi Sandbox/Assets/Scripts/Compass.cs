using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

    public GameObject compass_root;
    private GameObject tgt;
    public float rotationSpeed = .1f;

	// Update is called once per frame
	void LateUpdate () {

        Vector3 dir = (compass_root.transform.position - tgt.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        compass_root.transform.rotation = Quaternion.Slerp(compass_root.transform.rotation, rot, Time.deltaTime * rotationSpeed);
	}

    public void SetTarget(GameObject target)
    {
        tgt = target;
    }
}
