using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorreFase2 : MonoBehaviour
{
    //Seguir persoagem e animação de nocaute
    private Transform player;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    private bool rPressed = true;
    private bool tPressed = true;
    public float distAtaque;
    private bool podemorrer = true;
    //Script para contar capangas
    private TeletransportePorta teletransportePorta;
    

    void Start()
    {
        
            //Procura o jogador com a tag player para seguir
            navMeshAgent = GetComponent<NavMeshAgent>();

            GameObject jogador = GameObject.FindGameObjectWithTag("Player");
            GameObject scripttp = GameObject.FindGameObjectWithTag("scripttp");
            teletransportePorta = scripttp.GetComponent<TeletransportePorta>();

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
        SeguirEAnimar();

        //Vericação para saber se tomou nocaute
        if (Input.GetKeyDown(KeyCode.R))
            rPressed = false;

        if (Input.GetKeyDown(KeyCode.T))
            tPressed = false;

        TomarNocauteEParar();

    }

    private void TomarNocauteEParar()
    {
        //Tomar Nocaute e ficar parado
        float distancia = Vector3.Distance(transform.position, player.position);
        if (podemorrer)
            if (distancia <= distAtaque)
            {
                if (!rPressed || !tPressed)
                {
                    podemorrer = false;
                    Invoke("ResetaPodeMorrer", 1.5f);
                    animator.SetTrigger("nocaute");
                    navMeshAgent.isStopped = true;
                    rPressed = true;
                    tPressed = true;
                    navMeshAgent.speed = 0f;
                    teletransportePorta.ContarCapngasMortos();
                }
            }
    }

    private void SeguirEAnimar()
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

    //Reseta pode morrer
    void ResetaPodeMorrer()
    {
        podemorrer = true;
    }
}
