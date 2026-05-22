using UnityEngine;

public class EnemyWaypoints : MonoBehaviour
{
    public Color pathColor = Color.yellow;
    public Transform[] GetWaypoints()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return new Transform[] { transform }; 

        Transform[] points = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }
        return points;
    }
    private void OnDrawGizmos()
    {
        Transform[] points = GetWaypoints();
        if (points.Length < 2) return;
        Gizmos.color = pathColor;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
        Gizmos.DrawLine(points[points.Length - 1].position, points[0].position);
    }
}