using UnityEngine;

public class Launch : MonoBehaviour
{
    private Rigidbody rb;
    public ParticleSystem explosivePS;
    public float forceExplosion;
    public float radiusExplosion;
    
    [SerializeField] private GameObject direct;
    public float time = 0f;
    private bool isLaunched = false;

    private Trajectory trajectory;
    private Explosion explosionScript;
    public TrajectoryData tData = new TrajectoryData{};
    private HUD HUD;
    public LayerMask CheckMask;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        trajectory = GameManager.instance.trajectory;
        explosionScript = GetComponent<Explosion>();
        HUD = GameManager.instance.HUD;
        rb.useGravity = false;

        
        
    }

    private void Update(){
        
        if(Physics.CheckSphere(direct.transform.position, 0.4f, CheckMask)){
            isLaunched = false;
            HUD.NoSignalPanel.GetComponent<CanvasGroup>().alpha = 1;
            Instantiate(explosivePS, direct.transform.position, Quaternion.identity);
            explosivePS.Play();
            explosionScript.Explode(direct.transform, forceExplosion, radiusExplosion);
            Destroy(gameObject);
            trajectory.DestroyTrajectory();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            LaunchObj(tData.force);
        }
        if(isLaunched){
            time += Time.deltaTime;
            RotateToTrajectory(time+0.5f);
        }
        else{
            if(tData.force != HUD.force.value){
                tData.force = HUD.force.value;
                FixedTrajectoryValues();
            }
            else if(transform.localEulerAngles.x != HUD.vAngel.value ||
                    transform.localEulerAngles.y != HUD.hAngel.value) {
                transform.localEulerAngles = new Vector3(HUD.vAngel.value, HUD.hAngel.value, transform.localEulerAngles.z);
                FixedTrajectoryValues();
            }
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
        tData.speed = (tData.directPosition - tData.objPosition) * tData.force;
        trajectory.ShowTrajectory(tData.objPosition, tData.speed);
    }

}
