using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRotation : MonoBehaviour
{
    private HUD HUD;
    private Rocket rocket;
    

    private void Start() {
        HUD = GameManager.instance.HUD;
        rocket = GameManager.instance.rocket;
    }

    private void Update() {
        if(transform.localEulerAngles.y != HUD.hAngle.value
            && !rocket.isLaunched && rocket != null) {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, HUD.hAngle.value, transform.localEulerAngles.z);
                rocket.FixedTrajectoryValues();
            }
    }
}
