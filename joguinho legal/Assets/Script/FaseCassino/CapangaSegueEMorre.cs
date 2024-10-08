using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CapangaSegueEMorre : MonoBehaviour
{
    [Header("Referências")]
    private NavMeshAgent navMeshAgent; // Agente de navegação para o capanga
    public float distAtaque; // Distância para verificar o nocaute
    private VidaPersonagem vidaPersonagemMaisProxima;
    private bool podeAtacar = true;
    public bool morreu = false;

    //VIDA E DANO
    // Vida do vilão (campo privado)
    [SerializeField]
    private int _vidaVilao;
    public AudioSource somDor;

    // Propriedade pública para acessar e modificar a vida do vilão
    public int Vida
    {
        get { return _vidaVilao; }
        set
        {
            _vidaVilao = Mathf.Max(0, value); // Garante que a vida não seja negativa
            slidervidavilao.value = _vidaVilao; // Atualiza o slider
            Debug.Log("Vida do Vilão: " + _vidaVilao);
        }
    }

    // Vida e o slider dela
    public Slider slidervidavilao;
    private Animator animatorvilao;
    private Rigidbody rb;

    // Script VidaPersonagem
    public GameObject player;
    private VidaPersonagem vidaPersonagem;

    //patrulhar e ficar invisivel
    public Transform[] waypoints; // Pontos de patrulha
    private int currentWaypointIndex = 0; // Índice do waypoint atual
    public float patrolSpeed = 3f; // Velocidade de patrulha
    public float detectionRange = 10f; // Distância de detecção do jogador

    private FicarInvisivel ficarInvisivel;

    public int VidaInicial;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ficarInvisivel = player.GetComponent<FicarInvisivel>();

        // Inicializa o NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        animatorvilao = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        VidaInicial = Vida;
        // Inicializa a vida do vilão e configura o slider
        slidervidavilao.maxValue = Vida;
        slidervidavilao.value = Vida;
    }

    void Update()
    {
        // Encontrar o jogador mais próximo e seguir
        EncontrarPlayerMaisProximo();
        SeguirEanimar();
        if (ficarInvisivel != null)
        {
            PatrulharVerificar();
        }
        if (Vida <= 0)
        {
            VilaoMorreu();
        }
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
            float distanciaHorizontal = Vector3.Distance(
                new Vector3(posicaoCapanga.x, 0, posicaoCapanga.z),
                new Vector3(posicaoPlayer.x, 0, posicaoPlayer.z)
            );

            if (distanciaHorizontal < menorDistancia)
            {
                menorDistancia = distanciaHorizontal;
                jogadorMaisProximo = jogador.transform;
                vidaPersonagemMaisProxima = jogador.GetComponent<VidaPersonagem>(); // Armazena o VidaPersonagem do jogador mais próximo
            }
        }

        if (jogadorMaisProximo != null)
        {
            // Atualiza o destino do NavMeshAgent para seguir o jogador
            navMeshAgent.SetDestination(jogadorMaisProximo.position);

            // Verifica a distância entre o capanga e o jogador diretamente
            float distanciaAtaque = Vector3.Distance(
                transform.position,
                jogadorMaisProximo.position
            );

            if (distanciaAtaque <= distAtaque && podeAtacar && !morreu)
            {
                podeAtacar = false; // Impede novos ataques até o intervalo terminar
                DanoNoPlayer();
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
                Debug.Log(
                    "Atacando o jogador! Vida restante: " + vidaPersonagemMaisProxima.vidaAtual
                );
            }
            else
            {
                Debug.Log("Jogador fora da faixa de altura para ataque.");
            }
        }
    }

    private void SeguirEanimar()
    {
        // Controla a animação de andar/parar com base na velocidade do NavMeshAgent

        if (navMeshAgent.velocity != Vector3.zero)
        {
            animatorvilao.SetBool("andando", true); // Define o parâmetro "andando" como true
        }
        else
        {
            animatorvilao.SetBool("andando", false); // Define o parâmetro "andando" como false
        }
    }

    private void ResetarAtaque()
    {
        podeAtacar = true; // Permite que o capanga ataque novamente
    }

    // Receber dano e atualizar o slider
    public void ReceberDanoCapanga(int dano)
    {
        Vida -= dano;
        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", true);
        }
        Invoke("Resetou", 2);
        somDor.Play();

        
    }

    // Lógica quando o vilão morre
    public void VilaoMorreu()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0f;
        }

        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", false);
            animatorvilao.SetBool("nocaute", true);
        }
        rb.constraints = RigidbodyConstraints.FreezeAll;
        morreu = true;
    }

    public void Resetou()
    {
        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", false);
        }
    }

    public void PatrulharVerificar()
    {
        if (ficarInvisivel != null)
        {
            if (ficarInvisivel.isInvisible)
            {
                Patrulhar(); // Patrulha se o jogador estiver invisível
            }
            else
            {
                EncontrarPlayerMaisProximo();
                SeguirEanimar();
            }
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
                animatorvilao.SetBool("andando", true); // Define a animação "andando" como verdadeira
            }
            else
            {
                animatorvilao.SetBool("andando", false); // Define a animação "andando" como falsa
            }
        }
    }
}
