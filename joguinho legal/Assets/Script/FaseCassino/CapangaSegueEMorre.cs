using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorre : MonoBehaviour
{
    //Seguir persoagem e animação de nocaute
    private Transform player;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    private bool rPressed = true;
    private bool tPressed = true;
    public float distAtaque;

    void Start()
    {
        //Procura o jogador com a tag player para seguir
        navMeshAgent = GetComponent<NavMeshAgent>();

        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            player = jogador.transform;
        }
        else
        {
            Debug.LogError("Jogador não encontrado na cena!");
        }
 
    }
    void Update()
    {
        SeguirEanimar();

        //Vericação para saber se tomou nocaute
        if (Input.GetKeyDown(KeyCode.R))
            rPressed = false;

        if (Input.GetKeyDown(KeyCode.T))
            tPressed = false;

        VericarNocauteCapanga();
    }

    private void VericarNocauteCapanga()
    {
        //Tomar Nocaute e ficar parado
        float distancia = Vector3.Distance(transform.position, player.position);
        if (distancia <= distAtaque)
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
        //Seguir e animação
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
}
