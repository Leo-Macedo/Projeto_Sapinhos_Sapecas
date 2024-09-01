using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VidaPersonagem : MonoBehaviour
{
    [Header("Vida e Slider")]
    public float vidaAtual;
    private float vidaAnterior;
    private Animator animator;
    public bool acabouojogo = false;
    public Animator animatorCoracao;
    private Rigidbody rb;
    public float vidaInicial;

    [Header("Corações")]
    public int numMaximoCorações;
    public Image[] corações;
    public Sprite coraçãoCheio;
    public Sprite coraçãoVazio;

    void Start()
    {
        vidaInicial = vidaAtual;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vidaAnterior = Mathf.Ceil(vidaAtual); // Inicializa a vidaAnterior com o valor inteiro mais próximo de vidaAtual
    }

    void Update()
    {
        AtualizarCorações();
    }

    private void AtualizarCorações()
    {
        for (int i = 0; i < corações.Length; i++)
        {
            if (i < numMaximoCorações)
            {
                corações[i].sprite = (i < vidaAtual) ? coraçãoCheio : coraçãoVazio;
                corações[i].enabled = true;
            }
            else
            {
                corações[i].enabled = false;
            }
        }

        // Verifica se a vidaAtual perdeu exatamente 1 unidade cheia de vida
        if (Mathf.FloorToInt(vidaAtual) < Mathf.FloorToInt(vidaAnterior))
        {
            animatorCoracao.SetTrigger("tomou"); // Aciona a animação
            vidaAnterior = Mathf.Floor(vidaAtual); // Atualiza a vidaAnterior para o valor inteiro mais próximo de vidaAtual
        }
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, numMaximoCorações); // Garante que a vida esteja dentro dos limites
      
        Debug.Log("Vida do personagem: " + vidaAtual);
        
        if (vidaAtual <= 0)
        {
            animator.SetTrigger("nocaute");
                    rb.constraints = RigidbodyConstraints.FreezeAll;

           
        }
    }
}
