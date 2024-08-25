using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorreFase2 : MonoBehaviour
{
    [Header("Referências")]
    private Transform player; // Referência ao transform do jogador
    private NavMeshAgent navMeshAgent; // Componente para navegação em malha de navegação
    public Animator animator; // Componente Animator para animações

    public Transform[] waypoints; // Pontos de patrulha
    private int currentWaypointIndex = 0; // Índice do waypoint atual
    public float patrolSpeed = 3f; // Velocidade de patrulha
    public float detectionRange = 10f; // Distância de detecção do jogador

    private bool rPressed = true; // Controle do botão R
    private bool tPressed = true; // Controle do botão T
    public float distAtaque; // Distância de ataque
    private bool podemorrer = true; // Permite ao capanga morrer

    private TeletransportePorta teletransportePorta; // Referência ao script de teletransporte
    private FicarInvisivel ficarInvisivel; // Referência ao script de invisibilidade

    void Start()
    {
        // Inicializa componentes e procura o jogador e outros scripts necessários
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        GameObject scripttp = GameObject.FindGameObjectWithTag("scripttp");
        teletransportePorta = scripttp.GetComponent<TeletransportePorta>();

        if (jogador != null)
        {
            player = jogador.transform; // Obtém a posição do jogador
            ficarInvisivel = jogador.GetComponent<FicarInvisivel>(); // Obtém o componente de invisibilidade
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
                Patrulhar(); // Patrulha se o jogador estiver invisível
            }
            else
            {
                SeguirEAnimar(); // Segue e anima se o jogador não estiver invisível
            }
        }

        // Verifica se as teclas R e T foram pressionadas
        if (Input.GetKeyDown(KeyCode.R))
            rPressed = false;

        if (Input.GetKeyDown(KeyCode.T))
            tPressed = false;

        TomarNocauteEParar(); // Verifica se o capanga deve ser nocauteado
    }

    private void TomarNocauteEParar()
    {
        // Verifica se o capanga pode morrer e está próximo o suficiente para ser nocauteado
        float distancia = Vector3.Distance(transform.position, player.position);
        if (podemorrer && distancia <= distAtaque)
        {
            if (!rPressed || !tPressed)
            {
                podemorrer = false; // Desativa a capacidade de morrer
                Invoke("ResetaPodeMorrer", 1.5f); // Reativa a capacidade de morrer após 1.5 segundos
                animator.SetTrigger("nocaute"); // Aciona a animação de nocaute
                navMeshAgent.isStopped = true; // Para o agente de navegação
                navMeshAgent.speed = 0f; // Define a velocidade do agente como 0
                rPressed = true; // Reseta o controle do botão R
                tPressed = true; // Reseta o controle do botão T
            }
        }
    }

    private void SeguirEAnimar()
    {
        // Faz o capanga seguir o jogador e anima-lo
        navMeshAgent.SetDestination(player.position);

        if (navMeshAgent.velocity != Vector3.zero)
        {
            animator.SetBool("andando", true); // Define a animação "andando" como verdadeira
        }
        else
        {
            animator.SetBool("andando", false); // Define a animação "andando" como falsa
        }
    }

    private void Patrulhar()
    {
        // Faz o capanga patrulhar pelos waypoints
        if (waypoints.Length > 0)
        {
            navMeshAgent.speed = patrolSpeed; // Define a velocidade de patrulha
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

            // Verifica se chegou ao waypoint atual e muda para o próximo
            if (navMeshAgent.remainingDistance < 0.5f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }

            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetBool("andando", true); // Define a animação "andando" como verdadeira
            }
            else
            {
                animator.SetBool("andando", false); // Define a animação "andando" como falsa
            }
        }
    }

    private void ResetaPodeMorrer()
    {
        // Reativa a capacidade de morrer
        podemorrer = true;
    }
}
