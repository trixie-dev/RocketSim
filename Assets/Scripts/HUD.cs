using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text launcherCoordinate;
    public Toggle showTrajectory;
    // missle information
    public Text speed, alt, currTime;
    public GameObject NoSignalPanel;

    //control panel
    public Slider hAngle, vAngle;
    public Text forceText, hAngelText, vAngelText;

    // camera drone
    public Text droneAlt, droneDistance, coordinate, cross;
    public Slider zoom;

    // start information panel
    public Text distance, maxAlt, time;

    private void Update(){
        hAngelText.text = ((int)hAngle.value).ToString() + "°";
        vAngelText.text = ((int)-vAngle.value).ToString() + "°";
    }
}
