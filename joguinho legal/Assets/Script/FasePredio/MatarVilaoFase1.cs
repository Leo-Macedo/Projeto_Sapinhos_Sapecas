using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MatarVilaoFase1 : MonoBehaviour
{
    public GameObject melo;
    public GameObject portal;
    private VidaVilao vidaVilao;
    private MeloMovimentacao meloMovimentacao;
    public float alturaColisaoCabeça = 1.5f; // Altura para verificar se o jogador pulou na cabeça
    public bool podeatacar = true;

    void Start()
    {
        vidaVilao = melo.GetComponent<VidaVilao>();
        meloMovimentacao = melo.GetComponent<MeloMovimentacao>();
    }

    void LateUpdate()
    {
        if (vidaVilao.Vida <= 0)
        {
            Vitoria();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // Verifica se colidiu com a mosca
        if (other.gameObject.CompareTag("melo"))
        {
            Debug.Log("colidiu com melo");
            if (vidaVilao != null && meloMovimentacao.podeReceberDano)
            {
                // Verifica se o jogador está pulando na cabeça da mosca
                float alturaJogador = transform.position.y;
                float alturaMelo = melo.transform.position.y;
                Debug.Log($"Altura do jogador: {alturaJogador}, Altura do melo: {alturaMelo}");

                // Verifica se o jogador está pulando na cabeça da mosca
                if (alturaJogador > alturaMelo + alturaColisaoCabeça && podeatacar)
                {
                    // Aplica dano à mosca
                    vidaVilao.ReceberDanoVilao(1);
                    podeatacar = false;
                    Invoke("ResetaPodeAtacar", 2f);
                }
            }
        }
    }

    public void ResetaPodeAtacar()
    {
        podeatacar = true;
    }

    public void Vitoria()
    {
        // Marca a fase como completa no PlayerPrefs
        portal.SetActive(true);
        PlayerPrefs.SetInt("PredioCompletado", 1); // Define o progresso da fase
    }
}
