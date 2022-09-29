using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public LayerMask rocket;
    private bool isRocketHit;

    public ParticleSystem fire;
    public ParticleSystem smoke;
    private GameObject fireObj;
    private float capacity = 2700f;
    private bool updatingPosition = false;

    private void Update()
    {
        if(updatingPosition) {
            if(fire) fire.transform.position = transform.position;
            if(smoke) smoke.transform.position = transform.position;
        }
         
    }
    public void Hit(){
        fire = Instantiate(fire, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
        smoke = Instantiate(smoke, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
        updatingPosition = true;
        float k = transform.localScale.x * transform.localScale.y * transform.localScale.z / capacity;
        fire.transform.localScale = new Vector3(fire.transform.localScale.x*k, fire.transform.localScale.y*k, fire.transform.localScale.z*k);
        smoke.transform.localScale = new Vector3(smoke.transform.localScale.x*k, smoke.transform.localScale.y*k, smoke.transform.localScale.z*k);
        fire.Play();
        smoke.Play();
    }
    


}
