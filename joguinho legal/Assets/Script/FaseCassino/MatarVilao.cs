using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MatarVilao : MonoBehaviour
{
    [Header("Referências")]
    private Animator animator; // Animator do jogador
    public GameObject portalvoltar; // Portal de vitória
    public AudioSource somSoco;
    
    public int danoAtaque = 1;

    [Header("Melo Vilão")]
    public Animator animatorvilao; // Animator do vilão
    public GameObject vilao; // Objeto do vilão
    public Transform posicaovilao; // Posição do vilão
    public NavMeshAgent agentvilao; // NavMeshAgent do vilão

    [Header("Configurações de Ataque")]
   
    public bool podeatacar = true; // Flag para verificar se o ataque pode ser realizado
    public float distAtaque; // Distância do ataque
    public int vidavilao = 3; // Vida inicial do vilão

    [Header("Script Vida Vilão")]
    private VidaVilao vidaVilao; // Referência ao script de vida do vilão
    public bool socoExecutado = false; // Flag para verificar se o soco foi executado

    void Start()
    {
        // Referencia os componentes necessários
        animator = GetComponent<Animator>();
        animatorvilao = vilao.GetComponent<Animator>();
        vidaVilao = vilao.GetComponent<VidaVilao>();
        agentvilao = vilao.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Verifica a entrada do jogador para resetar o golpe
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            Invoke("ResetaGolpe", 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
           
            Invoke("ResetaGolpe", 1);
        }

        
    }

    public void DarDanoNoVilao()
    {
        // Aplica dano ao vilão e reseta flags
        if (socoExecutado)
        {
            float distancia = Vector3.Distance(transform.position, posicaovilao.position);
            if (distancia <= distAtaque && podeatacar)
            {
                Debug.Log("Chamo evento pela animação");
                vidaVilao.ReceberDanoVilao(danoAtaque);
                
                podeatacar = false;
                Invoke("PodeAtacar", 1);
            }

            socoExecutado = false; // Reseta o trigger do soco
        }
    }

    // Esta função deve ser chamada pelo AnimationEvent
    public void AcionarSoco()
    {
        somSoco.Play();
        socoExecutado = true; // Ativa o trigger quando a animação de soco acontece
    }

    // Reseta a animação de tomar soco
    

    // Permite que o jogador ataque novamente
    public void PodeAtacar()
    {
        podeatacar = true;
    }

    
}
