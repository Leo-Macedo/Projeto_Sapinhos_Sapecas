using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizarCasa : MonoBehaviour
{
    private Animator animator;

    [Header("Objetos")]
    public GameObject geladeira;
    public GameObject sofa;
    public GameObject privada;

    [Header("LugarNormal")]

    public Transform lugarGeladeira;
    public Transform lugarSofa;
    public Transform lugarPrivada;

    [Header("LugarMão")]

    public Transform lugarMaoGeladeira;
    public Transform lugarMaoSofa;
    public Transform lugarMaoPrivada;

    public bool pegouGeladeira;
    public bool pegouSofa;
    public bool pegouPrivada;
    public bool estaComObjeto;

    public GameObject txtColetar;
    public GameObject txtColocar;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        float inputX = animator.GetFloat("inputX");
        float inputY = animator.GetFloat("inputY");

        if (Mathf.Abs(inputX) == 0 || Mathf.Abs(inputY) == 0)
        {
            animator.SetBool("parou", true);
        }

        if (Mathf.Abs(inputX) != 0 || Mathf.Abs(inputY) != 0)
        {
            animator.SetBool("parou", false);
        }


        animator.SetBool("carregando", estaComObjeto);
    }

    private void OnTriggerStay(Collider other)
    {
        //Coletar itens

        if(other.gameObject.CompareTag("geladeira") && !pegouGeladeira)
        {
            txtColetar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !estaComObjeto)
            {
                estaComObjeto = true;
                pegouGeladeira = true;
                geladeira.transform.SetParent(transform);
                geladeira.transform.position = lugarMaoGeladeira.transform.position;
                geladeira.transform.rotation = lugarMaoGeladeira.transform.rotation;
            }

        }

        else if (other.gameObject.CompareTag("sofa") && !pegouSofa)
        {
            txtColetar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !estaComObjeto)
            {
                estaComObjeto = true;
                pegouSofa = true;
                sofa.transform.SetParent(transform);
                sofa.transform.position = lugarMaoSofa.transform.position;
                sofa.transform.rotation = lugarMaoSofa.transform.rotation;
            }

        }

        else if (other.gameObject.CompareTag("privada") && !pegouPrivada)
        {
            txtColetar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !estaComObjeto)
            {
                estaComObjeto = true;                                                              
                pegouSofa = true;
                sofa.transform.SetParent(transform);
                sofa.transform.position = lugarMaoSofa.transform.position;
                sofa.transform.rotation = lugarMaoSofa.transform.rotation;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("geladeira") && !pegouGeladeira)
        {
            txtColetar.SetActive(true);

        }

        else if (other.gameObject.CompareTag("sofa") && !pegouSofa)
        {
            txtColetar.SetActive(true);

        }

        else if (other.gameObject.CompareTag("privada") && !pegouPrivada)
        {
            txtColetar.SetActive(true);

        }
    }
}
