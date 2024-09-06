using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MatarVilaoFase1 : MonoBehaviour
{
    [Header("Referências")]
    public GameObject vilao; // Referência ao objeto do vilão
    private Animator animatorvilao; // Componente Animator do vilão
    private Animator animator; // Componente Animator do jogador (não utilizado no código atual)

    public int vidaVilao = 3; // Vida inicial do vilão

    private bool podepular = true; // Permite ou não pular no vilão
    public GameObject portalvoltar; // Portal para voltar à cena anterior
    public Transform tpAndar2;
    public GameObject player;

    void Start()
    {
        // Obtém o componente Animator do vilão
        animatorvilao = vilao.GetComponent<Animator>();
        animator = GetComponent<Animator>(); // Obtém o componente Animator do jogador
    }

    void Update()
    {
        // Verifica se a vida do vilão chegou a 0 e executa a função de morte e vitória
        if (vidaVilao <= 0)
        {
            VilaoMorreu(); // Lida com a morte do vilão
            Vitoria(); // Marca a fase como completa
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica se o jogador está em um objeto com a tag "lugar"
        if (other.gameObject.CompareTag("lugar"))
        {
            animatorvilao.SetBool("parado", true); // Define a animação do vilão para "parado"
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Verifica se o jogador colidiu com a cabeça do vilão e se pode pular
        if (other.gameObject.CompareTag("cabeca") && podepular)
        {
            DarDanoPulandoNaCabeca(); // Dá dano ao vilão ao pular na cabeça
        }
    }

    private void VilaoMorreu()
    {
        // Define animações e ativa o portal de volta
        animatorvilao.SetTrigger("nocaute"); // Aciona a animação de nocaute
        animatorvilao.SetBool("caiu", false); // Define a animação "caiu" como falsa
        Debug.Log("VilaoMorreu"); // Loga a morte do vilão
        portalvoltar.SetActive(true); // Ativa o portal para voltar à cena anterior
    }

    private void DarDanoPulandoNaCabeca()
    {
        // Reduz a vida do vilão e atualiza o estado de pular
        animatorvilao.SetBool("caiu", true); // Define a animação "caiu" como verdadeira
        vidaVilao -= 1; // Reduz a vida do vilão
        podepular = false; // Desativa a capacidade de pular temporariamente
        Invoke("PodePular", 2f); // Reativa a capacidade de pular após 2 segundos
        Debug.Log("Vida do Vilão: " + vidaVilao); // Loga a vida restante do vilão
    }

    public void PodePular()
    {
        // Reativa a capacidade de pular e redefine a animação do vilão
        podepular = true; // Permite pular novamente
        animatorvilao.SetBool("caiu", false); // Define a animação "caiu" como falsa
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("escada"))
        {
            player.transform.position = tpAndar2.position;
        }
    }

    public void Vitoria()
    {
        // Marca a fase como completa no PlayerPrefs
        PlayerPrefs.SetInt("PredioCompletado", 1); // Define o progresso da fase
    }
}
