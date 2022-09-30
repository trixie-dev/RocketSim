using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown;
    [SerializeField] private DroneCamera drone;
    [SerializeField] private string direction;
    private void Update()
    {
        if(isDown) drone.CamRotate(direction);
    }
    public void OnPointerDown(PointerEventData eventData){
        isDown = true;
    }
    
    public void OnPointerUp (PointerEventData eventData)  
    {  
        isDown = false;  
    }  

}
