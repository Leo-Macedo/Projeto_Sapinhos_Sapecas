using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ChutarBola : MonoBehaviour
{
    public float forcaChute = 10f; // Força do chute
    public float distanciaChute = 3f; // Distância máxima para chutar a bomba
    private Animator animator;
    private GameObject bombaMaisProxima; // Referência à bomba mais próxima

    // Referências para a câmera e o jogador
    public CinemachineFreeLook cinemachineCamera; // Câmera FreeLook
    public Transform jogador; // Referência ao jogador
    public Transform vilao; // Referência ao vilão

    void Start(){
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // Verifica se o jogador pressionou um botão para buscar a bomba mais próxima
        if (Input.GetKeyDown(KeyCode.F)) // Por exemplo, botão esquerdo do mouse
        {
            animator.SetTrigger("chutar");
            EncontrarBombaMaisProxima();
        }
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
            float distancia = Vector3.Distance(transform.position, bombaMaisProxima.transform.position);
            if (distancia <= distanciaChute) // Verifica se está dentro da distância
            {
                // Aplica força contrária à bomba
                Rigidbody rb = bombaMaisProxima.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direcaoChute = (bombaMaisProxima.transform.position - transform.position).normalized;
                    rb.AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
                    StartCoroutine(MudarCameraParaPlayer());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bomba"))
        {
            StartCoroutine(MudarCameraParaVilao());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("bomba"))
        {
            StartCoroutine(MudarCameraParaPlayer());
        }
    }

    private IEnumerator MudarCameraParaVilao()
    {
        cinemachineCamera.LookAt = vilao; // Muda o follow para o vilão
        yield return new WaitForSeconds(0.1f); // Espera 0.1 segundos
        cinemachineCamera.enabled = true; // Muda o follow de volta para o jogador
    }

    private IEnumerator MudarCameraParaPlayer()
    {
        cinemachineCamera.LookAt = jogador; // Muda o follow para o vilão
        yield return new WaitForSeconds(0.1f); // Espera 0.1 segundos
        cinemachineCamera.enabled = true; // Muda o follow de volta para o jogador
    }
}
