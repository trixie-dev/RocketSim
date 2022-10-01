using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Rocket rocket;
    public Trajectory trajectory;
    public Explosion explosionScript;
    public HUD HUD;
    public int rocketCount;
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
