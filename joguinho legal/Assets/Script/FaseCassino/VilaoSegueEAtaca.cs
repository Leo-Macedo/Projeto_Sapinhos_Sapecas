using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class VilaoSegueEAtaca : MonoBehaviour
{
    //Referencia ao jogador
    public Animator animatorPlayer;
    public Transform player;
    private AçõesPersonagem açõesPersonagem;
    //Scripts
    private VidaVilao vidaVilao;
    private VidaPersonagem vidaPersonagemScript;

    //Seguir o jogador
    public float chaseInterval = 5f;
    public float stopDuration = 3f;

    private NavMeshAgent navMeshAgent;
    public bool isChasing = true;
    public Animator animator;
    public float distAtaque;

    public bool podeatacar = true;



    void Start()
    {
        //Referencia aos componentes
        vidaPersonagemScript = player.GetComponent<VidaPersonagem>();
        vidaVilao = GetComponent<VidaVilao>();

        if (vidaPersonagemScript != null)
            vidaPersonagemScript.vida = 3; // Define a vida inicial do personagem aqui

        açõesPersonagem = player.GetComponent<AçõesPersonagem>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Inicia corrotina para seguir 
        StartCoroutine(ChasePlayer());
    }

    //Segue o jogador e para a partir dos intervalos
    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if (isChasing)
            {
                navMeshAgent.SetDestination(player.position);
                yield return new WaitForSeconds(chaseInterval);
                isChasing = false;
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(stopDuration);
                isChasing = true;
                navMeshAgent.isStopped = false;
            }
        }
    }

    void Update()
    {
        Seguiranimar();
        

        float distancia = Vector3.Distance(transform.position, player.position);
        if (!vidaPersonagemScript.acabouojogo)
        {
            if (distancia <= distAtaque)
            {
                animator.SetBool("ataque", true);
                Invoke("NãoPodeAtacar", 1);
            }
        }

    }
    public void AtacarOJogador()
    {
        //Atacar o jogador
       
           
                if (podeatacar)
                {
                    podeatacar = false;
              
                    VerificarDesvio();
                    Invoke("PodeAtacar", 5);
                    
                }
            
    }

    private void Seguiranimar()
    {
        //Animações de andar e parar

        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
        }
    }

    //Verifica se o jogador desviou do golpe
    public void VerificarDesvio()
    {
        if (!açõesPersonagem.EstaDesviando())
        {
            animatorPlayer.SetBool("caiu", true);
            Invoke("ResetouCaiu", 2);
            vidaPersonagemScript.ReceberDano(1);
        }
        else
        {
            Debug.Log("Jogador desviou do ataque");
        }
    }
    //Resetar animação de ataque
    public void NãoPodeAtacar()
    {
        animator.SetBool("ataque", false);

    }
    //Poder atacar novamente
    public void PodeAtacar()
    {
        podeatacar = true;
    }
    //Resetar animação de tomar soco
    public void ResetouCaiu()
    {
        animatorPlayer.SetBool("caiu", false);
    }
}
