using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class CirculoTrilho : MonoBehaviour
{
    public CinemachineSmoothPath path;
    public float radius = 10000f; // Raio do círculo
    public int numberOfWaypoints = 9; // Número de waypoints

    void UpdatePath()
    {
        if (path == null)
            path = GetComponent<CinemachineSmoothPath>();

        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[numberOfWaypoints];

        for (int i = 0; i < numberOfWaypoints; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfWaypoints;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            path.m_Waypoints[i].position = position;
        }
        
        path.InvalidateDistanceCache(); // Atualiza o cache de distância
    }

    void OnValidate()
    {
        UpdatePath();
    }
}
