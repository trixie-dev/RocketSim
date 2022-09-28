using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Vector3 centerOfMass;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        rb.centerOfMass = centerOfMass;
        rb.WakeUp();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
