using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VilaoSegueEAtaca : MonoBehaviour
{
    [Header("Referências do Jogador")]
    public Transform player; // Transform do jogador
    
    [Header("Scripts")]
    private VidaVilao vidaVilao; // Script de vida do vilão
    public VidaPersonagem vidaPersonagemScript; // Script de vida do jogador

    [Header("Configurações de Perseguição")]
    public float chaseInterval = 5f; // Intervalo de perseguição
    public float stopDuration = 3f; // Duração da pausa na perseguição

    private NavMeshAgent navMeshAgent; // NavMeshAgent do vilão
    public bool isChasing = true; // Flag para verificar se está perseguindo
    public Animator animator; // Animator do vilão
    public float distAtaque; // Distância do ataque
    public bool podeatacar = true; // Flag para verificar se pode atacar

    void Start()
    {
        // Referencia os componentes necessários
        vidaPersonagemScript = player.GetComponent<VidaPersonagem>();
        vidaVilao = GetComponent<VidaVilao>();

        if (vidaPersonagemScript != null)
            vidaPersonagemScript.vidaAtual = 3; // Define a vida inicial do personagem

        navMeshAgent = GetComponent<NavMeshAgent>();

        // Inicia a corrotina para seguir o jogador
        StartCoroutine(ChasePlayer());
    }

    // Corrotina para seguir o jogador e pausar periodicamente
    IEnumerator ChasePlayer()
    {
        while (true)
        {
            if (isChasing)
            {
                navMeshAgent.SetDestination(player.position); // Define o destino do vilão
                yield return new WaitForSeconds(chaseInterval);
                isChasing = false; // Para de perseguir
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(stopDuration);
                isChasing = true; // Retoma a perseguição
                navMeshAgent.isStopped = false;
            }
        }
    }

    void Update()
    {
        Seguiranimar(); // Atualiza a animação com base no movimento

        
    }

  

    private void Seguiranimar()
    {
        // Atualiza a animação de andar/parar com base na velocidade do NavMeshAgent
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
