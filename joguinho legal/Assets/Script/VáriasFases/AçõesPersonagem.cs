using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AçõesPersonagem : MonoBehaviour
{
    //Animações de Ação do personagem
    private Animator animator;
    private bool estadesviando = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger("soco");


        if (Input.GetKeyDown(KeyCode.T))
            animator.SetTrigger("chute");


        if (Input.GetKeyDown(KeyCode.E))
            animator.SetTrigger("emote1");

        if (Input.GetKeyDown(KeyCode.G))
            Desvio();


    }

    void Desvio()
    {
        estadesviando = true;
        animator.SetTrigger("desvio");
        Invoke("PararDesvio", 2); // Supondo que o desvio dura 1 segundo
    }

    void PararDesvio()
    {
        estadesviando = false;
    }

    public bool EstaDesviando()
    {
        return estadesviando;
    }
}
