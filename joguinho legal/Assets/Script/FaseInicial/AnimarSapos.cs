using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimarSapos : MonoBehaviour
{
    private Animator animator;
    private Vector3 ultimaPosicao;
    private Vector3 movimento;
    

    void Start()
    {
        ultimaPosicao = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimacaoEAndando();
        farpar();
    }

    private void AnimacaoEAndando()
    {
        movimento = transform.position - ultimaPosicao;

        ultimaPosicao = transform.position;

        if (movimento != Vector3.zero)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
        }
    }

    public void farpar(){
    //Farpar Inimigo
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("emote1");
        }
    }
}