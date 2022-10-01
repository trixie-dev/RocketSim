using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rb;
    public ParticleSystem explosivePS;
    public float forceExplosion;
    public float radiusExplosion;
    
    [SerializeField] private GameObject direct;
    public float time = 0f;
    public bool isLaunched = false;

    private Trajectory trajectory;
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
        HUD.launcherCoordinate.text = "own's coordinate: " + String(transform.position.x) + " x, " + String(transform.position.z) + " y";
        rb.useGravity = false;
    
    }

    private void Update(){

        showTrajectory = HUD.showTrajectory.isOn;

        if(Physics.CheckSphere(direct.transform.position, 0.4f, CheckMask)){

            isLaunched = false;

            HUD.rocketCount.text = (GameManager.instance.rocketCount - 1).ToString();
            HUD.speed.text = "--";
            HUD.speed.color = Color.red;
            HUD.alt.text = "--";
            HUD.alt.color = Color.red;
            HUD.currTime.text = "--";
            HUD.currTime.color = Color.red;
            HUD.NoSignalPanel.GetComponent<CanvasGroup>().alpha = 1;

            Instantiate(explosivePS, direct.transform.position, Quaternion.identity);
            explosivePS.Play();
            explosionScript.Explode(direct.transform, forceExplosion, radiusExplosion);
            Destroy(gameObject);
            trajectory.DestroyTrajectory();

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
        CalcStartData();

    }

    public void CalcStartData(){

        float distance = Mathf.Pow(tData.speed.magnitude, 2)*Mathf.Sin(2*-tData.rotation.x*Mathf.PI/180)/-Physics.gravity.y;
        float maxAlt = (Mathf.Pow(tData.speed.magnitude, 2) * Mathf.Pow(Mathf.Sin(-tData.rotation.x*Mathf.PI/180), 2)/-2*Physics.gravity.y/100f)+tData.objPosition.y;
        float time = 2 * tData.speed.magnitude*Mathf.Sin(-tData.rotation.x*Mathf.PI/180)/-Physics.gravity.y;

        HUD.distance.text = distance.ToString();
        HUD.maxAlt.text = maxAlt.ToString();
        HUD.time.text = time.ToString();

    }


    private string String(float num){
        return ((int)num).ToString();
    }
}
