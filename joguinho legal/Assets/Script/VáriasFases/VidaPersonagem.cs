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

    //Corações
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Start()
    {
        //Pegar animator 
        animator = GetComponent<Animator>();
        
    }

     void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

            if (vida > numOfHearts)
            {
                vida = numOfHearts;
            }

            if (i < vida)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;

            }
        }

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
