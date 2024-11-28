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

    private float inicialAndando;
    private float inicialCorrendo;
    private Movimento2 movimento2;

    public bool simGeladeira;
    public bool simSofa;
    public bool simPrivada;

    void Start()
    {
        animator = GetComponent<Animator>();    
        movimento2 = GetComponent<Movimento2>();

        inicialAndando = movimento2.veloAndando;
        inicialCorrendo = movimento2.veloCorrendo;
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
        if(estaComObjeto)
        {
            movimento2.veloAndando = 1;
            movimento2.veloCorrendo = 1;

        }
        else if (!estaComObjeto)
        {
            movimento2.veloAndando = inicialAndando;
            movimento2.veloCorrendo = inicialCorrendo;

        }




        animator.SetBool("carregando", estaComObjeto);
    }

    private void OnTriggerStay(Collider other)
    {
        //Coletar itens

        if(other.gameObject.CompareTag("geladeira") && !pegouGeladeira && !simGeladeira)
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

        else if (other.gameObject.CompareTag("sofa") && !pegouSofa && !simSofa)
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

        else if (other.gameObject.CompareTag("privada") && !pegouPrivada && !simPrivada)
        {
            txtColetar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !estaComObjeto)
            {
                estaComObjeto = true;                                                              
                pegouPrivada = true;
                privada.transform.SetParent(transform);
                privada.transform.position = lugarMaoPrivada.transform.position;
                privada.transform.rotation = lugarMaoPrivada.transform.rotation;
            }

        }

        // Deixar Itens

        if (other.gameObject.CompareTag("geladeiralugar") && pegouGeladeira)
        {
            txtColocar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && estaComObjeto && pegouGeladeira && !simGeladeira)
            {
                estaComObjeto = false;
                simGeladeira = true;
                txtColocar.SetActive(false);
                geladeira.transform.SetParent(null); 
                geladeira.transform.position = lugarGeladeira.transform.position;
                geladeira.transform.rotation = lugarGeladeira.transform.rotation;
            }

        }

        if (other.gameObject.CompareTag("geladeiralugar") && pegouGeladeira)
        {
            txtColocar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && estaComObjeto && pegouGeladeira && !simGeladeira)
            {
                estaComObjeto = false;
                simGeladeira = true;
                txtColocar.SetActive(false);
                geladeira.transform.SetParent(null);
                geladeira.transform.position = lugarGeladeira.transform.position;
                geladeira.transform.rotation = lugarGeladeira.transform.rotation;
            }

        }

        if (other.gameObject.CompareTag("geladeiralugar") && pegouGeladeira)
        {
            txtColocar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && estaComObjeto && pegouGeladeira && !simGeladeira)
            {
                estaComObjeto = false;
                simGeladeira = true;
                txtColocar.SetActive(false);
                geladeira.transform.SetParent(null);
                geladeira.transform.position = lugarGeladeira.transform.position;
                geladeira.transform.rotation = lugarGeladeira.transform.rotation;
            }

        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("geladeira"))
        {
            txtColetar.SetActive(false);

        }

        else if (other.gameObject.CompareTag("sofa"))
        {
            txtColetar.SetActive(false);

        }

        else if (other.gameObject.CompareTag("privada"))
        {
            txtColetar.SetActive(false);

        }

        if (other.gameObject.CompareTag("lugargeladeira"))
        {
            txtColocar.SetActive(false);

        }

        else if (other.gameObject.CompareTag("lugarsofa"))
        {
            txtColocar.SetActive(false);

        }

        else if (other.gameObject.CompareTag("lugarprivada"))
        {
            txtColocar.SetActive(false);

        }
    }

    
}
