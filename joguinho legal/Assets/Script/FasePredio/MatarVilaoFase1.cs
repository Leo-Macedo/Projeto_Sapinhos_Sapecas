using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MatarVilaoFase1 : MonoBehaviour
{
    public GameObject vilao;
    private Animator animatorvilao;
    private Animator animator;

    public int vidaVilao = 3;

    private bool podepular = true;
    public GameObject portalvoltar;

    void Start()
    {
        //Referencia os componentes
        animatorvilao = vilao.GetComponent<Animator>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("lugar"))
            animatorvilao.SetBool("parado", true);



    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("cabeca") && podepular)
        {
            animatorvilao.SetBool("caiu", true);
            vidaVilao -= 1;
            podepular = false;
            Invoke("PodePular", 2f);
            Debug.Log("VilaoMorreu" + vidaVilao);
        }

        if (vidaVilao <= 0)
        {
            animatorvilao.SetTrigger("nocaute");
            animatorvilao.SetBool("caiu", false);
            Debug.Log("VilaoMorreu");
            portalvoltar.SetActive(true);

        }
    }

    public void PodePular()
    {
        podepular = true;
        animatorvilao.SetBool("caiu", false);

    }



}
