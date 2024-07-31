using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoAndaFase2 : MonoBehaviour
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
}