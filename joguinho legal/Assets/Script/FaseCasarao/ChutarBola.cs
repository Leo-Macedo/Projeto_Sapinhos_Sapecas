using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChutarBola : MonoBehaviour
{
    public GameObject txtChutar;
    public float forcaChute = 10f; // Força do chute
    public float distanciaChute = 3f; // Distância máxima para chutar a bomba
    private Animator animator;
    private GameObject bombaMaisProxima; // Referência à bomba mais próxima

    // Referências para a câmera e o jogador
    public CinemachineFreeLook cinemachineCamera; // Câmera FreeLook
    public Transform jogador; // Referência ao jogador
    public Transform vilao; // Referência ao vilão
    public Transform pontoBola;
    public LineRenderer lineRenderer; // Referência ao LineRenderer
    public float maxDistance = 100f; // Distância máxima do raycast
    public LayerMask collisionMask; // Máscara de camada para detectar colisões
    public float alturaInclinação = 2f; // Altura da inclinação da linha
    public bool podeGrudar = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Verifica se o jogador pressionou um botão para buscar a bomba mais próxima
        if (Input.GetButtonDown("Interagir")) // Por exemplo, botão esquerdo do mouse
        {
            animator.SetTrigger("chutar");
            EncontrarBombaMaisProxima();
            lineRenderer.enabled = false;
            txtChutar.SetActive(false);
            // Desativar a linha ao chutar
        }

        CriarMira();
    }

    // Método para encontrar a bomba mais próxima
    private void EncontrarBombaMaisProxima()
    {
        // Encontra todas as bombas com a tag "bomba"
        GameObject[] bombas = GameObject.FindGameObjectsWithTag("bomba");
        float menorDistancia = Mathf.Infinity; // Inicializa a menor distância como infinita

        // Itera sobre todas as bombas para encontrar a mais próxima
        foreach (GameObject bomba in bombas)
        {
            float distancia = Vector3.Distance(transform.position, bomba.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                bombaMaisProxima = bomba; // Atualiza a bomba mais próxima
            }
        }
    }

    // Método a ser chamado pelo evento da animação
    public void ChutarBomba()
    {
        if (bombaMaisProxima != null)
        {
            float distancia = Vector3.Distance(
                transform.position,
                bombaMaisProxima.transform.position
            );
            if (distancia <= distanciaChute) // Verifica se está dentro da distância
            {
                // Aplica força na direção da linha
                Rigidbody rb = bombaMaisProxima.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calcular direção da linha
                    Vector3 direcaoChute = (
                        lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)
                    ).normalized;
                    rb.AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
                }
            }
            podeGrudar = true;
        }
        bombaMaisProxima.AddComponent<CaixaDanoNoVilao>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bomba") && podeGrudar)
        {
            GrudarBolaNoPe(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("bomba"))
        {
            lineRenderer.enabled = false;
            other.transform.SetParent(null);
            podeGrudar = true;
            txtChutar.SetActive(false);
        }
    }

    public void GrudarBolaNoPe(Collider other)
    {
        CaixaDanoNoPlayer caixaDanoNoPlayer = other.GetComponent<CaixaDanoNoPlayer>();
        Rigidbody rb = other.GetComponent<Rigidbody>();
        txtChutar.SetActive(true);
        rb.velocity = Vector3.zero; // Zera a velocidade

        if (caixaDanoNoPlayer != null)
        {
            if (caixaDanoNoPlayer.jaColidiu)
            {
                rb.velocity = Vector3.zero; // Zera a velocidade

                other.transform.position = pontoBola.position;
                other.transform.SetParent(transform);
                other.transform.rotation = pontoBola.rotation;
                lineRenderer.enabled = true;
                podeGrudar = false; // Ativa o LineRenderer ao grudar a bomba
            }
        }
        else if (caixaDanoNoPlayer == null)
        {
            rb.velocity = Vector3.zero; // Zera a velocidade

            other.transform.position = pontoBola.position;
            other.transform.rotation = pontoBola.rotation;
            other.transform.SetParent(transform);
            lineRenderer.enabled = true;
            podeGrudar = false; // Ativa o LineRenderer ao grudar a bomba
        }
    }

    private void CriarMira()
    {
        // Define o ponto de origem da linha (pode ser a posição do objeto)
        Vector3 origem = pontoBola.position;

        // Define a direção para onde a linha será desenhada (pode ser o vetor "frente" do objeto)
        Vector3 direcao = transform.forward;

        // Realiza um raycast na direção definida
        RaycastHit hit;
        if (Physics.Raycast(origem, direcao, out hit, maxDistance, collisionMask))
        {
            // Se uma colisão for detectada, atualiza o ponto final da linha
            lineRenderer.SetPosition(0, origem); // Ponto inicial
            lineRenderer.SetPosition(1, hit.point + Vector3.up * alturaInclinação); // Ponto final na colisão com um deslocamento para cima
        }
        else
        {
            // Se não houver colisão, estenda a linha até o infinito (ou a distância máxima)
            lineRenderer.SetPosition(0, origem);
            lineRenderer.SetPosition(
                1,
                origem + direcao * maxDistance + Vector3.up * alturaInclinação
            ); // Altera aqui se quiser uma linha "infinita"
        }
    }
}
