using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo;
    public Transform posicaoCamera;
    public float ajusteCamera = 0.3f;
    public float suavidade = 5f; // Controla a suavidade da transição
    public float distanciaMinima = 0.2f; // Distância mínima para evitar tremores
    public float rotationSpeed = 2f; // Velocidade de rotação vertical da câmera

    private Vector3 posicaoDestino;
    private bool emColisao;
    private float rotacaoVertical = 0f;

    void Start()
    {
        posicaoDestino = posicaoCamera.position; // Posição inicial da câmera
    }

    void Update()
    {
        AjustarPosicaoCamera();
        RotacionarCamera();
    }

    private void AjustarPosicaoCamera()
    {
        Vector3 destinoCamera = posicaoCamera.position;

        if (Physics.Linecast(alvo.position, posicaoCamera.position, out RaycastHit hit))
        {
            emColisao = true;

            destinoCamera = hit.point + hit.normal * ajusteCamera;

            posicaoDestino = Vector3.Lerp(posicaoDestino, destinoCamera, Time.deltaTime * suavidade);
        }
        else
        {
            if (emColisao)
            {
                emColisao = false;
                posicaoDestino = posicaoCamera.position;
            }
        }

        transform.position = emColisao ? posicaoDestino : destinoCamera;
    }

    private void RotacionarCamera()
{
    // Captura o movimento vertical do mouse
    float mouseYInput = Input.GetAxis("CameraY");
    
    // Ignorar pequenas entradas
    if (Mathf.Abs(mouseYInput) > 0.01f)
    {
        // Acumula a rotação vertical
        rotacaoVertical -= mouseYInput * rotationSpeed;

        // Limita a rotação vertical para evitar que a câmera "vire de cabeça para baixo"
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -20f, 20f);

        // Aplica a rotação apenas no eixo X
        transform.localRotation = Quaternion.Euler(rotacaoVertical, 0f, 0f);
    }
}
}
