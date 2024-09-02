using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VilaoSegueEAtaca : MonoBehaviour
{
    [Header("Referências do Jogador")]
    public Animator animatorPlayer; // Animator do jogador
    public Transform player; // Transform do jogador
    private AçõesPersonagem açõesPersonagem; // Script de ações do jogador
    
    [Header("Scripts")]
    private VidaVilao vidaVilao; // Script de vida do vilão
    private VidaPersonagem vidaPersonagemScript; // Script de vida do jogador

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

        açõesPersonagem = player.GetComponent<AçõesPersonagem>();
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

        float distancia = Vector3.Distance(transform.position, player.position);
        if (!vidaPersonagemScript.acabouojogo)
        {
            if (distancia <= distAtaque)
            {
                animator.SetBool("ataque", true); // Inicia a animação de ataque
                Invoke("NãoPodeAtacar", 1); // Reseta a animação de ataque após 1 segundo
            }
        }
    }

    public void AtacarOJogador()
    {
        // Ataca o jogador se estiver dentro da distância de ataque e se for permitido atacar
        float distancia = Vector3.Distance(transform.position, player.position);
        if (distancia <= distAtaque)
        {
            if (podeatacar)
            {
                podeatacar = false; // Desativa a possibilidade de atacar

                VerificarDesvio(); // Verifica se o jogador desviou do ataque
                Invoke("PodeAtacar", 1); // Permite atacar novamente após 5 segundos
            }
        }
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

    public void VerificarDesvio()
    {
        // Verifica se o jogador desviou e aplica dano se não tiver desviado
        if (!açõesPersonagem.EstaDesviando())
        {
         vidaPersonagemScript.ReceberDano(1); // Aplica dano ao jogador
        }
        else
        {
            Debug.Log("Jogador desviou do ataque");
        }
    }

    public void NãoPodeAtacar()
    {
        // Reseta a animação de ataque
        animator.SetBool("ataque", false);
    }

    public void PodeAtacar()
    {
        // Permite que o vilão ataque novamente
        podeatacar = true;
    }

   
}
