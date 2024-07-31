using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VidaPersonagem : MonoBehaviour
{
    //Vida e o slider dela
    public int vida;
    public Slider sliderVida;
    private Animator animator;
    public bool acabouojogo = false;
    public GameObject txtPerdeu;
    void Start()
    {
        //Pegar animator 
        animator = GetComponent<Animator>();
    }

    //Receber dano e morrer
    public void ReceberDano(int dano)
    {
        vida -= dano;
        sliderVida.value = vida;
        Debug.Log("Vida do personagem: " + vida);

        if (vida <= 0)
        {   animator.SetTrigger("nocaute");
            txtPerdeu.SetActive(true);
            acabouojogo = true;
        }
    }
}
