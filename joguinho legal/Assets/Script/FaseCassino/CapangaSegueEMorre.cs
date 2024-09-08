using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapangaSegueEMorre : MonoBehaviour
{
    [Header("Referências")]
    private NavMeshAgent navMeshAgent; // Agente de navegação para o capanga
    private Animator animator; // Controlador de animação do capanga
    public float distAtaque; // Distância para verificar o nocaute
    private VidaPersonagem vidaPersonagemMaisProxima;
    private bool podeAtacar = true;

    void Start()
    {
        // Inicializa o NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Encontrar o jogador mais próximo e seguir
        EncontrarPlayerMaisProximo();
        SeguirEanimar();
    }

   private void EncontrarPlayerMaisProximo()
{
    GameObject[] jogadores = GameObject.FindGameObjectsWithTag("Player");
    Transform jogadorMaisProximo = null;
    float menorDistancia = Mathf.Infinity;

    foreach (GameObject jogador in jogadores)
    {
        Vector3 posicaoPlayer = jogador.transform.position;
        Vector3 posicaoCapanga = transform.position;
        
        // Calcula a distância no plano XZ
        float distanciaHorizontal = Vector3.Distance(new Vector3(posicaoCapanga.x, 0, posicaoCapanga.z), new Vector3(posicaoPlayer.x, 0, posicaoPlayer.z));
        
        if (distanciaHorizontal < menorDistancia)
        {
            menorDistancia = distanciaHorizontal;
            jogadorMaisProximo = jogador.transform;
            vidaPersonagemMaisProxima = jogador.GetComponent<VidaPersonagem>(); // Armazena o VidaPersonagem do jogador mais próximo
        }
    }

    if (jogadorMaisProximo != null)
    {
        navMeshAgent.SetDestination(jogadorMaisProximo.position);

        if (navMeshAgent.remainingDistance <= distAtaque && podeAtacar)
        {
            podeAtacar = false; // Impede novos ataques até o intervalo terminar
            Invoke("DanoNoPlayer", 0f);
            Invoke("ResetarAtaque", 5f); // Permite o próximo ataque após 5 segundos
        }
    }
}

    private void DanoNoPlayer()
{
    if (vidaPersonagemMaisProxima != null)
    {
        Vector3 posicaoPlayer = vidaPersonagemMaisProxima.transform.position;
        Vector3 posicaoCapanga = transform.position;

        // Verifica se o jogador está na mesma faixa de altura (ajuste o valor de 1.0f conforme necessário)
        if (Mathf.Abs(posicaoPlayer.y - posicaoCapanga.y) < 1.0f)
        {
            vidaPersonagemMaisProxima.vidaAtual -= 0.1f;
            Debug.Log("Atacando o jogador! Vida restante: " + vidaPersonagemMaisProxima.vidaAtual);
        }
        else
        {
            Debug.Log("Jogador fora da faixa de altura para ataque.");
        }
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

    private void ResetarAtaque()
    {
        podeAtacar = true; // Permite que o capanga ataque novamente
    }
}
