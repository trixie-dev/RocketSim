using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    private Rigidbody rb;
    public TrajectoryRenderer Trajectory;
    public TrajectoryRenderer Tangent;
    [SerializeField] private float force;
    [SerializeField] private GameObject direct;
    public float time = 0f;
    private Vector3 speed;
    private bool isLaunched = false;


    private Vector3 startPosition;
    private Vector3 directStartPosition;

    private Vector3 startRotation;
    private Vector3 startSpeed;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        //LaunchObj(direct, force);
        startPosition = transform.position;
        directStartPosition = direct.transform.position;
        startRotation = transform.localEulerAngles;
        startSpeed = (directStartPosition-startPosition) * force;
        Debug.Log(startSpeed.y * Mathf.Sin(startRotation.y*Mathf.PI/180)/-Physics.gravity.y);
        
        }

    private void Update(){
        speed = (direct.transform.position-transform.position) * force;
        Vector3 startSpeed = (directStartPosition - startPosition) * force;
        Trajectory.ShowTrajectory(startPosition, startSpeed);

        if(Input.GetKeyDown(KeyCode.Space)){
            LaunchObj(direct, force);
        }
        if(isLaunched){
            time += Time.deltaTime;
            RotateToTrajectory(time+0.2f);
            
            //RotateToTrajectory(time);
        }
    }   
    private void RotateToTrajectory(float time){
        Vector3 position = startPosition + startSpeed * time + Physics.gravity * time * time / 2f;
        transform.LookAt(position);
    }
    
    private void LaunchObj(GameObject obj, float force){
        isLaunched = true;
        rb.useGravity = true;
        Vector3 direction = obj.transform.position - transform.position;
        rb.AddForce(direction*force, ForceMode.VelocityChange);
    }


}
