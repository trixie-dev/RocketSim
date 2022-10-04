using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Rocket rocket;
    public DroneCamera drone;
    public Trajectory trajectory;
    public Explosion explosionScript;
    public PlayerCamera playerCamera;
    public HUD HUD;
    public int rocketCount;
    public int rocketsNumbers;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
    }
}
