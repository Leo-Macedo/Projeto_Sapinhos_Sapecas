using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VilaoAtacaPlayer : MonoBehaviour
{
    [Header("Referências do Jogador")]
    public Transform player; // Transform do jogador
    private AçõesPersonagem açõesPersonagem; // Script de ações do jogador
    private MeloMovimentacao meloMovimentacao; // Script de movimentação (MeloMovimentacao)

    [Header("Scripts")]
    private VidaVilao vidaVilao; // Script de vida do vilão
    private VidaPersonagem vidaPersonagemScript; // Script de vida do jogador

    public Animator animator; // Animator do vilão
    public float distAtaque; // Distância do ataque
    public bool podeatacar = true; // Flag para verificar se pode atacar

    void Start()
    {
        // Referencia os componentes necessários
        vidaPersonagemScript = player.GetComponent<VidaPersonagem>();
        vidaVilao = GetComponent<VidaVilao>();
        animator = GetComponent<Animator>();
        açõesPersonagem = player.GetComponent<AçõesPersonagem>();

        // Tenta encontrar o script MeloMovimentacao no jogador
        meloMovimentacao = GetComponent<MeloMovimentacao>();
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, player.position);
        if (!vidaPersonagemScript.acabouojogo)
        {
            // Se o script MeloMovimentacao existir, verifica se o jogador está voando
            if (distancia <= distAtaque && (meloMovimentacao == null || meloMovimentacao.voando))
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
        if (distancia <= distAtaque && (meloMovimentacao == null || meloMovimentacao.voando))
        {
            if (podeatacar)
            {
                podeatacar = false; // Desativa a possibilidade de atacar

                VerificarDesvio(); // Verifica se o jogador desviou do ataque
                Invoke("PodeAtacar", 1); // Permite atacar novamente após 5 segundos
            }
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
