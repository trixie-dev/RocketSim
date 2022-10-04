using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rb;
    public int numberOfRocket;
    public ParticleSystem explosivePS;
    public float forceExplosion;
    public float radiusExplosion;
    
    [SerializeField] private GameObject direct;
    public float time = 0f;
    public bool isLaunched = false;

    private Trajectory trajectory;
    private DroneCamera drone;
    public bool showTrajectory;
    private Explosion explosionScript;
    public TrajectoryData tData = new TrajectoryData{};
    private HUD HUD;
    public LayerMask CheckMask;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        trajectory = GameManager.instance.trajectory;
        explosionScript = GetComponent<Explosion>();
        HUD = GameManager.instance.HUD;
        drone = GameManager.instance.drone;
        HUD.launcherCoordinate.text = "own's coordinate: " + String(transform.position.x) + " x, " + String(transform.position.z) + " y";
        rb.useGravity = false;
    
    }

    private void Update(){
        print(tData.speed.magnitude);
        showTrajectory = HUD.showTrajectory.isOn;

        if(Physics.CheckSphere(direct.transform.position, 4f, CheckMask)){
            
            isLaunched = false;

            
            HUD.speed.text = "--";
            HUD.speed.color = Color.red;
            HUD.alt.text = "--";
            HUD.alt.color = Color.red;
            HUD.currTime.text = "--";
            HUD.currTime.color = Color.red;
            HUD.NoSignalPanel.GetComponent<CanvasGroup>().alpha = 1;
            HUD.rocketCount.text = (GameManager.instance.rocketCount - 1).ToString();

            Instantiate(explosivePS, direct.transform.position, Quaternion.identity);
            explosivePS.Play();
            explosionScript.Explode(direct.transform, forceExplosion, radiusExplosion);
           
            trajectory.DestroyTrajectory();
            Destroy(gameObject);

        }
        if(Input.GetKeyDown(KeyCode.Space))
            LaunchObj();
        
        if(isLaunched){

            if(showTrajectory) trajectory.ShowTrajectoryByTime(tData.objPosition, tData.speed, time);
            time += Time.deltaTime;
            RotateToTrajectory(time+0.5f);

        }
        
        else{

            if(transform.localEulerAngles.x != HUD.vAngle.value ||
                transform.localEulerAngles.y != HUD.hAngle.value) {

                transform.localEulerAngles = new Vector3(HUD.vAngle.value, HUD.hAngle.value, transform.localEulerAngles.z);
                FixedTrajectoryValues();

            }
        }
        // updating HUD information
        HUD.speed.text = String(rb.velocity.magnitude) + " km/h";
        HUD.alt.text = String(transform.position.y) + " m";
        HUD.currTime.text = time.ToString() + " s";

    }   
    private void RotateToTrajectory(float time){

        Vector3 position = tData.objPosition + tData.speed * time + Physics.gravity * time * time / 2f;
        transform.LookAt(position);
    }
    
    public void LaunchObj(){

        isLaunched = true;
        rb.useGravity = true;
        Vector3 direction = tData.directPosition - tData.objPosition;
        rb.AddForce(direction*tData.force, ForceMode.VelocityChange);
        
    }

    public void FixedTrajectoryValues(){

        tData.objPosition = transform.position;
        tData.directPosition = direct.transform.position;
        tData.rotation = transform.localEulerAngles;
        tData.speed = (tData.directPosition - tData.objPosition) * tData.force;
        HUD.forceText.text = String(tData.speed.magnitude) + "km/h";
        if(showTrajectory) trajectory.ShowStartTrajectory(tData.objPosition, tData.speed);
        UpdateStartDataInHUD(CalcStartTime(), CalcStartDistance());

    }

    public void CalcStartData(){

        float distance = Mathf.Pow(tData.speed.magnitude, 2)*Mathf.Sin(2*-tData.rotation.x*Mathf.PI/180)/-Physics.gravity.y;
        float maxAlt = (Mathf.Pow(tData.speed.magnitude, 2) * Mathf.Pow(Mathf.Sin(-tData.rotation.x*Mathf.PI/180), 2)/-2*Physics.gravity.y/100f)+tData.objPosition.y;
        float time = 2 * tData.speed.magnitude*Mathf.Sin(-tData.rotation.x*Mathf.PI/180)/-Physics.gravity.y;

        HUD.distance.text = distance.ToString();
        //HUD.maxAlt.text = maxAlt.ToString();
        //HUD.time.text = time.ToString();


    }


    private string String(float num){
        return ((int)num).ToString();
    }

    private float CalcStartTime(){
        if(drone.isFocused){
            float ownHeight = transform.position.y;
            float targetHeight = drone.targetHeight;
            float sinA = Mathf.Sin(-tData.rotation.x * Mathf.PI / 180f);
            float v0 = tData.speed.magnitude;
            float g = - Physics.gravity.y;
            float result;

            if(ownHeight > targetHeight)
                result = (v0 * sinA + Mathf.Sqrt(Mathf.Pow(v0, 2) * Mathf.Pow(sinA, 2) + Mathf.Abs(2 * g * (ownHeight - targetHeight)))) / g;
            else
                result = (v0 * sinA + Mathf.Sqrt(Mathf.Pow(v0, 2) * Mathf.Pow(sinA, 2) - Mathf.Abs(2 * g * (ownHeight - targetHeight)))) / g;
            
            return result;
        }
        
        return -1488f;
    }
    private void UpdateStartDataInHUD(float time, float distance){

        
        
        // distance
        if(!drone.isFocused){
            HUD.time.text = "no data";
            HUD.distance.text = "no data";
        }
        else{

            // time
            if(float.IsNaN(time)) 
                HUD.time.text = "NaN";
            else
                HUD.time.text = time.ToString();

            HUD.distance.text = distance.ToString();
        }
        

        
    }
    private float CalcStartDistance(){
        float time = CalcStartTime();
        if(drone.isFocused && time != -1488f){
            float result = tData.speed.magnitude * Mathf.Cos(-tData.rotation.x * Mathf.PI / 180f) * time;
            return result;
        }
        return -1488;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(direct.transform.position, 4f);
        Gizmos.DrawWireSphere(direct.transform.position, 4f);
    }
}
