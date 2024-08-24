using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorreFase2 : MonoBehaviour
{
    // Seguir personagem e animação de nocaute
    private Transform player;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    public Transform[] waypoints; // Pontos de patrulha
    private int currentWaypointIndex = 0;
    public float patrolSpeed = 3f; // Velocidade de patrulha
    public float detectionRange = 10f; // Distância de detecção do jogador

    private bool rPressed = true;
    private bool tPressed = true;
    public float distAtaque;
    private bool podemorrer = true;

    private TeletransportePorta teletransportePorta;
    private FicarInvisivel ficarInvisivel;

    void Start()
    {
        // Procura o jogador com a tag player para seguir
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        GameObject scripttp = GameObject.FindGameObjectWithTag("scripttp");
        teletransportePorta = scripttp.GetComponent<TeletransportePorta>();

        if (jogador != null)
        {
            player = jogador.transform;
            ficarInvisivel = jogador.GetComponent<FicarInvisivel>();
        }
        else
        {
            Debug.LogError("Jogador não encontrado na cena!");
        }
    }

    void Update()
    {
        if (ficarInvisivel != null)
        {
            if (ficarInvisivel.isInvisible)
            {
                Patrulhar();
            }
            else
            {
                SeguirEAnimar();
            }
        }

        // Verificação para saber se tomou nocaute
        if (Input.GetKeyDown(KeyCode.R))
            rPressed = false;

        if (Input.GetKeyDown(KeyCode.T))
            tPressed = false;

        TomarNocauteEParar();
    }

    private void TomarNocauteEParar()
    {
        // Tomar Nocaute e ficar parado
        float distancia = Vector3.Distance(transform.position, player.position);
        if (podemorrer && distancia <= distAtaque)
        {
            if (!rPressed || !tPressed)
            {
                podemorrer = false;
                Invoke("ResetaPodeMorrer", 1.5f);
                animator.SetTrigger("nocaute");
                navMeshAgent.isStopped = true;
                navMeshAgent.speed = 0f;
                rPressed = true;
                tPressed = true;
            }
        }
    }

    private void SeguirEAnimar()
    {
        // Seguir e animação
        navMeshAgent.SetDestination(player.position);

        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
        }
    }

    private void Patrulhar()
    {
        if (waypoints.Length > 0)
        {
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

            if (navMeshAgent.remainingDistance < 0.5f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }

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

    // Reseta pode morrer
    void ResetaPodeMorrer()
    {
        podemorrer = true;
    }
}
