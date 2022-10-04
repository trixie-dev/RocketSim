using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Vector3 pOffset;
    private Camera droneCamera;
    public bool isFocused = false;
    public float rotateSpeed;
    private HUD HUD;
    private ButtonScript button;
    public RaycastHit hit;
    public float targetHeight;
    private Vector3 focusPoint = Vector3.zero;
    
    
    private void Start() {
        HUD = GameManager.instance.HUD;
        droneCamera = GetComponent<Camera>();
        button = GetComponent<ButtonScript>();

    }
    void Update()
    {
        droneCamera.fieldOfView = HUD.zoom.value;
        HUD.droneAlt.text = "Alt: " + ((int)transform.position.y).ToString() + "m";
        InvokeRepeating("ChangePosition", 0, 3f);
        if(Input.GetKeyDown(KeyCode.F) && isFocused) isFocused = false;
        else if(Input.GetKeyDown(KeyCode.F) && !isFocused) isFocused = true; 

        focus(isFocused);
        if(isFocused == false) {
            HUD.coordinate.text = "Coordinate: -- x, -- y";
            HUD.droneDistance.text = "Distance: -- m";
            HUD.cross.color = new Color(0.2f, 0.2f, 0.2f);
            focusPoint = Vector3.zero;
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        transform.Rotate(0, rotateSpeed*x, 0);
        transform.Rotate(rotateSpeed*y, 0, 0);
        
    }

    private void ChangePosition(){
        Vector3 newPos = transform.position + new Vector3(Random.Range(-pOffset.x, pOffset.x), Random.Range(-pOffset.y, pOffset.y), Random.Range(-pOffset.z, pOffset.z));
        transform.position = Vector3.Slerp(transform.position, newPos, 3f);
    }

    public Vector3 SetFocus(){
        Ray ray = new Ray(transform.position, transform.forward * 1000);
        Debug.DrawLine(transform.position, transform.position + transform.forward * 1000, Color.red);

        if(Physics.Raycast(ray, out hit, 1000))
            HUD.cross.color = new Color(1f, 0f, 0f);
            HUD.coordinate.text = "Coordinate: " + ((int)hit.point.x).ToString() + " x, " + ((int)hit.point.z).ToString() + " y";
            HUD.droneDistance.text = "Distance: " + ((int)hit.distance).ToString() + " m";
            targetHeight = hit.point.y;
            return hit.point;
        

    }
    public void focus(bool isFocused){
        if(isFocused) {
            if(focusPoint == Vector3.zero) focusPoint = SetFocus();
            transform.LookAt(focusPoint);             
        }
    }
    public void CamRotate(string direction){
        float x = 0, y = 0;
        
        if(direction == "left") x = -1;
        else if(direction == "right") x = 1;
        else if(direction == "up") y = -1;
        else if(direction == "down") y = 1;
        transform.Rotate(0, rotateSpeed*x, 0);
        transform.Rotate(rotateSpeed*y, 0, 0);
    }
    public void ChangeFocusStatus(){
        isFocused = !isFocused;
    }
}
