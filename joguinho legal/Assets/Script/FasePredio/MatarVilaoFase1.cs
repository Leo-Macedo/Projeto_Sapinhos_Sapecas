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

    private Rigidbody rbJogador; // Referência ao Rigidbody do jogador
    public float forcaRecuo = 5f; // Força de recuo para trás

    void Start()
    {
        vidaVilao = melo.GetComponent<VidaVilao>();
        meloMovimentacao = melo.GetComponent<MeloMovimentacao>();
        rbJogador = GetComponent<Rigidbody>(); // Obtém o Rigidbody do jogador
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
        if (other.gameObject.CompareTag("melo"))
        {
            Debug.Log("colidiu com melo");
            if (vidaVilao != null && meloMovimentacao.podeReceberDano)
            {
                float alturaJogador = transform.position.y;
                float alturaMelo = melo.transform.position.y;
                Debug.Log($"Altura do jogador: {alturaJogador}, Altura do melo: {alturaMelo}");

                if (alturaJogador > alturaMelo + alturaColisaoCabeça && podeatacar)
                {
                    vidaVilao.ReceberDanoVilao(1);

                    // Calcula a direção de recuo para trás
                    Vector3 direcaoRecuo = (transform.position - melo.transform.position).normalized;
                    direcaoRecuo.y = 0; // Mantém a força apenas no plano horizontal
                    rbJogador.AddForce(direcaoRecuo * forcaRecuo, ForceMode.Impulse);

                    podeatacar = false;
                    Invoke("ResetaPodeAtacar", 2f);
                    meloMovimentacao.TomarDano();
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
        portal.SetActive(true);
        PlayerPrefs.SetInt("PredioCompletado", 1);
    }
}
