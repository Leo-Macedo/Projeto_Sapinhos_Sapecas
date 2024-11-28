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

    [Header("LugarM�o")]
    public Transform lugarMaoGeladeira;
    public Transform lugarMaoSofa;
    public Transform lugarMaoPrivada;

    [Header("Referencias")]
    public GameObject painelSenha;
    public GameObject txtColetar;
    public GameObject txtColocar;
    public AudioSource somNoti;
    public GameObject[] mensagem;
    public Animator[] animatorMSG;
    public float veloCarregando = 2;

    private bool pegouGeladeira;
    private bool pegouSofa;
    private bool pegouPrivada;
    private bool estaComObjeto;
    private bool organizandoCasa = false;

    private float inicialAndando;
    private float inicialCorrendo;
    private Movimento2 movimento2;

    private bool simGeladeira;
    private bool simSofa;
    private bool simPrivada;

    void Start()
    {
        animator = GetComponent<Animator>();
        movimento2 = GetComponent<Movimento2>();

        inicialAndando = movimento2.veloAndando;
        inicialCorrendo = movimento2.veloCorrendo;
    }

    void Update()
    {
        SincronizarAnimacao();

        if (simGeladeira && simPrivada && simSofa && !organizandoCasa)
        {
            StartCoroutine(OrganizouCasa());
            organizandoCasa = true;
        }
    }

    private IEnumerator OrganizouCasa()
    {
        painelSenha.SetActive(true);
        somNoti.Play();
        mensagem[0].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[0].SetTrigger("fechou");
        somNoti.Play();
        mensagem[1].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[1].SetTrigger("fechou");
    }

    private void SincronizarAnimacao()
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
        if (estaComObjeto)
        {
            movimento2.veloAndando = veloCarregando;
            movimento2.veloCorrendo = veloCarregando;
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

        if (other.gameObject.CompareTag("geladeira") && !pegouGeladeira && !simGeladeira)
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
        else if (other.gameObject.CompareTag("sofalugar") && pegouSofa)
        {
            txtColocar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && estaComObjeto && pegouSofa && !simSofa)
            {
                estaComObjeto = false;
                simSofa = true;
                txtColocar.SetActive(false);
                sofa.transform.SetParent(null);
                sofa.transform.position = lugarSofa.transform.position;
                sofa.transform.rotation = lugarSofa.transform.rotation;
            }
        }
        else if (other.gameObject.CompareTag("privadalugar") && pegouPrivada)
        {
            txtColocar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && estaComObjeto && pegouPrivada && !simPrivada)
            {
                estaComObjeto = false;
                simPrivada = true;
                txtColocar.SetActive(false);
                privada.transform.SetParent(null);
                privada.transform.position = lugarPrivada.transform.position;
                privada.transform.rotation = lugarPrivada.transform.rotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (
            other.gameObject.CompareTag("geladeira")
            || other.gameObject.CompareTag("sofa")
            || other.gameObject.CompareTag("privada")
            || other.gameObject.CompareTag("geladeiralugar")
            || other.gameObject.CompareTag("sofalugar")
            || other.gameObject.CompareTag("privadalugar")
        )
        {
            txtColetar.SetActive(false);
        }
    }
}
