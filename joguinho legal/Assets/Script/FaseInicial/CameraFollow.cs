using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O jogador
    public Vector3 offset; // Offset da câmera em relação ao jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização da câmera
    public float minDistance = 1.5f; // Distância mínima da câmera ao jogador
    public float maxDistance = 5f; // Distância máxima da câmera ao jogador

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calcula a posição desejada da câmera
            Vector3 desiredPosition = target.position + offset;

            // Verifica se há colisões
            Vector3 direction = (desiredPosition - target.position).normalized;
            float distance = Vector3.Distance(desiredPosition, target.position);

            // Se a distância for maior que o máximo permitido, ajusta a posição
            if (distance > maxDistance)
            {
                desiredPosition = target.position + direction * maxDistance;
            }

            // Verifica colisões entre a posição desejada e o jogador
            RaycastHit hit;
            if (Physics.Raycast(target.position, -direction, out hit, distance))
            {
                // Se houver uma colisão, ajusta a posição da câmera para a distância mínima
                desiredPosition = hit.point + direction * minDistance;
            }

            // Move a câmera suavemente em direção à posição desejada
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.LookAt(target.position); // Faz a câmera olhar para o jogador
        }
    }
}
