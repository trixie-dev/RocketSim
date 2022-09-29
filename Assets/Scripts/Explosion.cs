using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void Explode(Transform obj, float force, float radius){
        Collider[] overlappedColliders = Physics.OverlapSphere(obj.position, radius);

        for(int i = 0; i < overlappedColliders.Length; i++) {
            Rigidbody rb = overlappedColliders[i].attachedRigidbody;
            if(rb && rb.tag != "Rocket"){
                rb.AddExplosionForce(force, obj.position, radius);
                overlappedColliders[i].gameObject.SendMessage("Hit");
            }
        }
    }
}
