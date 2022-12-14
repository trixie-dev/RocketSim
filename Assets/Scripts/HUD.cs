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

    // missiles panel
    public Text rocketCount;
    private void Start(){
        rocketCount.text = GameManager.instance.rocketCount.ToString();   
    }
    private void Update(){
        hAngelText.text = (hAngle.value).ToString() + "°";
        vAngelText.text = (-vAngle.value).ToString() + "°";
    }
}
