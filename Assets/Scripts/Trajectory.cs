using UnityEngine;

public class Trajectory : MonoBehaviour
{
    private LineRenderer lineRendererComponent;

    private void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
    }

    public void ShowStartTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[20000];

        lineRendererComponent.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

            if(points[i].y < 0 )
            {
                lineRendererComponent.positionCount = i+1;
                break;
            }
        }

        lineRendererComponent.SetPositions(points);
    }
    public void ShowTrajectoryByTime(Vector3 origin, Vector3 speed, float time)
    {
        Vector3[] points = new Vector3[20000];

        lineRendererComponent.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float tmp_time = i * 0.1f+ time;
            points[i] = origin + speed * tmp_time + Physics.gravity * tmp_time * tmp_time / 2f;

            if(points[i].y < 0 )
            {
                lineRendererComponent.positionCount = i+1;
                break;
            }
        }

        lineRendererComponent.SetPositions(points);
    }

    public void DestroyTrajectory(){
        Destroy(gameObject);
    }

}
