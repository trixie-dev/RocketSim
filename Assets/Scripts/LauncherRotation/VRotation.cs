using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRotation : MonoBehaviour
{
    private HUD HUD;
    private Rocket rocket;

    private void Start() {
        HUD = GameManager.instance.HUD;
        rocket = GameManager.instance.rocket;
    }

    private void Update() {
        if(transform.localEulerAngles.x != HUD.vAngle.value 
            && !rocket.isLaunched && rocket != null) {
                transform.localEulerAngles = new Vector3(-HUD.vAngle.value, transform.localEulerAngles.y, transform.localEulerAngles.z);
                rocket.FixedTrajectoryValues();
            }
    }
}
