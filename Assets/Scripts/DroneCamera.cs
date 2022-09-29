using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Vector3 pOffset;
    private bool isFocused = false;
    public float rotateSpeed;
    private HUD HUD;
    // Update is called once per frame
    private void Start() {
        HUD = GameManager.instance.HUD;

    }
    void Update()
    {
        HUD.droneAlt.text = "Alt: " + ((int)transform.position.y).ToString() + "m";
        InvokeRepeating("ChangePosition", 0, 3f);
        if(Input.GetKeyDown(KeyCode.F) && isFocused) isFocused = false;
        else if(Input.GetKeyDown(KeyCode.F) && !isFocused) isFocused = true; 

        focus(isFocused);
        if(isFocused == false) {
            HUD.coordinate.text = "Coordinate: -- x, -- y";
            HUD.distance.text = "Distance: -- m";
            HUD.cross.color = new Color(0.2f, 0.2f, 0.2f);
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

    private Vector3 SetFocus(){
        Ray ray = new Ray(transform.position, transform.forward * 1000);
        Debug.DrawLine(transform.position, transform.position + transform.forward * 1000, Color.red);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
            HUD.cross.color = new Color(1f, 0f, 0f);
            HUD.coordinate.text = "Coordinate: " + ((int)hit.transform.position.x).ToString() + " x, " + ((int)hit.transform.position.z).ToString() + " y";
            HUD.distance.text = "Distance: " + ((int)hit.distance).ToString() + " m";
            return hit.transform.position;
        //return Vector3.zero;

    }
    private void focus(bool isFocused){
        if(isFocused) {
            Vector3 position = SetFocus();
            if(position != Vector3.zero) 
                transform.LookAt(position);
                
        }
        
        
    }
}
