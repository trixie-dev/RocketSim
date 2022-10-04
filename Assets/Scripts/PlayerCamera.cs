using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Rocket rocket;
    private bool following = false;
    private Vector3 startPosition;
    public Vector3 preLaunchRotation;
    [SerializeField] private Vector3 offset;

    private void Start() {
        rocket = GameManager.instance.rocket;
        startPosition = transform.position;
        
    }
    private void Update() {
        if(following && rocket != null && rocket.isLaunched){
            transform.position = rocket.transform.position + offset;
            transform.rotation = rocket.transform.rotation;
        }
        else if(rocket == null || (!following && rocket.isLaunched)){
            transform.position = startPosition;
            transform.localEulerAngles = preLaunchRotation;
        }
    }

     void FollowRocket(){
        following = !following; 
    }
}
