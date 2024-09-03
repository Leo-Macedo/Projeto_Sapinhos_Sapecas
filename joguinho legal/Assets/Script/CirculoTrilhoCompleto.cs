using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class CirculoTrilhoCompleto : MonoBehaviour
{
    public CinemachineSmoothPath path;
    public float radius = 10000f; // Raio do círculo
    public int numberOfWaypoints = 9; // Número de waypoints (excluindo o último ponto que fecha o círculo)

    void UpdatePath()
    {
        if (path == null)
            path = GetComponent<CinemachineSmoothPath>();

        // Adicionar um waypoint extra para fechar o círculo
        path.m_Waypoints = new CinemachineSmoothPath.Waypoint[numberOfWaypoints + 1];

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

        // Fechar o círculo: o último waypoint deve ser igual ao primeiro
        path.m_Waypoints[numberOfWaypoints].position = path.m_Waypoints[0].position;

        path.InvalidateDistanceCache(); // Atualiza o cache de distância
    }

    void OnValidate()
    {
        UpdatePath();
    }
}
