using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

    public GameObject compassRoot;
    private GameObject tgt;
    public float rotationSpeed = 0.1f;

    // Update is called once per frame
    void LateUpdate () {

        Vector3 dir = (compassRoot.transform.position - tgt.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        compassRoot.transform.rotation = Quaternion.Slerp(compassRoot.transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    public void SetTarget(GameObject target)
    {
        tgt = target;
    }
}
