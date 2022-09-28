using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private float force;
    [SerializeField] private GameObject direct;
    public float time = 0f;
    private bool isLaunched = false;

    private Trajectory trajectory;
    private TrajectoryData tData = new TrajectoryData{};
    private HUD HUD;
    public LayerMask CheckMask;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        trajectory = GameManager.instance.trajectory;
        HUD = GameManager.instance.HUD;
        rb.useGravity = false;

        FixedTrajectoryValues();
        
    }

    private void Update(){
        if(Physics.CheckSphere(direct.transform.position, 0.4f, CheckMask)){
            isLaunched = false;
            HUD.NoSignalPanel.GetComponent<CanvasGroup>().alpha = 1;
            //Destroy(gameObject);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            LaunchObj(force);
        }
        if(isLaunched){
            time += Time.deltaTime;
            RotateToTrajectory(time+0.5f);
        }
        
        // updating HUD information
        HUD.speed.text = "Speed: " + ((int)rb.velocity.magnitude).ToString() + " km/h";
        HUD.alt.text = "Alt: " + ((int)transform.position.y).ToString() + " m";
        HUD.time.text = "Time: " + ((int)time).ToString() + " s";
    }   
    private void RotateToTrajectory(float time){
        Vector3 position = tData.objPosition + tData.speed * time + Physics.gravity * time * time / 2f;
        transform.LookAt(position);
    }
    
    private void LaunchObj(float force){
        isLaunched = true;
        rb.useGravity = true;
        Vector3 direction = tData.directPosition - tData.objPosition;
        rb.AddForce(direction*force, ForceMode.VelocityChange);
    }

    public void FixedTrajectoryValues(){
        tData.objPosition = transform.position;
        tData.directPosition = direct.transform.position;
        tData.rotation = transform.localEulerAngles;
        tData.speed = (tData.directPosition - tData.objPosition) * force;
        trajectory.ShowTrajectory(tData.objPosition, tData.speed);
    }

}
