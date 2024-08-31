using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VidaPersonagem : MonoBehaviour
{
    [Header("Vida e Slider")]
    public int vidaAtual;
    private Animator animator;
    public bool acabouojogo = false;
    public GameObject txtPerdeu;
    public Animator animatorCoracao;

    [Header("Corações")]
    public int numMaximoCorações;
    public Image[] corações;
    public Sprite coraçãoCheio;
    public Sprite coraçãoVazio;

    void Start()
    {
        animator = GetComponent<Animator>();
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
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, numMaximoCorações); // Garante que a vida esteja dentro dos limites
      
        Debug.Log("Vida do personagem: " + vidaAtual);
        animatorCoracao.SetTrigger("tomou");
        if (vidaAtual <= 0)
        {
            animator.SetTrigger("nocaute");
            txtPerdeu.SetActive(true);
            acabouojogo = true;
        }
    }
}
