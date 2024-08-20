using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorre : MonoBehaviour
{
    //Seguir personagem e animação de nocaute
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    private bool rPressed = true;
    private bool tPressed = true;
    public float distAtaque;

    void Start()
    {
        // Inicializa o NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Encontrar e seguir o jogador mais próximo
        EncontrarPlayerMaisProximo();
        SeguirEanimar();

        // Verificação para saber se tomou nocaute
        if (Input.GetKeyDown(KeyCode.R))
            rPressed = false;

        if (Input.GetKeyDown(KeyCode.T))
            tPressed = false;

        VericarNocauteCapanga();
    }

    private void EncontrarPlayerMaisProximo()
    {
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

    private void VericarNocauteCapanga()
    {
        // Tomar Nocaute e ficar parado
        if (navMeshAgent.remainingDistance <= distAtaque)
        {
            if (!rPressed || !tPressed)
            {
                animator.SetTrigger("nocaute");
                navMeshAgent.isStopped = true;
                rPressed = true;
                tPressed = true;
                navMeshAgent.speed = 0f;
            }
        }
    }

    private void SeguirEanimar()
    {
        // Animação de andar/parar
        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
        }
    }
}
