using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorre : MonoBehaviour
{
    [Header("Referências")]
    private NavMeshAgent navMeshAgent; // Agente de navegação para o capanga
    public Animator animator; // Controlador de animação do capanga
    public float distAtaque; // Distância para verificar o nocaute

    void Start()
    {
        // Inicializa o NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Encontrar o jogador mais próximo e seguir
        EncontrarPlayerMaisProximo();
        SeguirEanimar();
    }

    private void EncontrarPlayerMaisProximo()
    {
        // Encontra o jogador mais próximo e define o destino do NavMeshAgent
        GameObject[] jogadores = GameObject.FindGameObjectsWithTag("Player");
        Transform jogadorMaisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject jogador in jogadores)
        {
            float distancia = Vector3.Distance(transform.position, jogador.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                jogadorMaisProximo = jogador.transform;
            }
        }

        if (jogadorMaisProximo != null)
        {
            navMeshAgent.SetDestination(jogadorMaisProximo.position);
        }
    }

    public void VericarNocauteCapanga()
    {
        // Verifica se o capanga está dentro da distância de ataque e aplica o nocaute
        if (navMeshAgent.remainingDistance <= distAtaque)
        {
            animator.SetTrigger("nocaute"); // Aciona a animação de nocaute
            navMeshAgent.isStopped = true; // Para o movimento do NavMeshAgent
            navMeshAgent.speed = 0f; // Define a velocidade como 0
        }
    }

    private void SeguirEanimar()
    {
        // Controla a animação de andar/parar com base na velocidade do NavMeshAgent
        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("andando", true); // Define o parâmetro "andando" como true
        }
        else
        {
            animator.SetBool("andando", false); // Define o parâmetro "andando" como false
        }
    }
}
