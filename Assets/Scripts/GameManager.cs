using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Trajectory trajectory;
    public HUD HUD;
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
