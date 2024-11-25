using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PassarFasesCasarao : MonoBehaviour
{
    public VerificarFasesCasarao VerificarFasesCasarao;
    public GameObject DebugChave;
    public bool estaComChave;
    public bool completouSenha;
    public bool abriu;
    public GameObject porta; // Porta a ser desbloqueada
    public Animator animatoPorta;
    public TextMeshProUGUI debugPorta;
    public GameObject debugPortaobj;
    public AudioSource som;
    public AudioSource somChave;
    private bool flag1;
    private bool flag2;
    private bool flag3;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("portaporao") && !flag1)
        {
            if (VerificarFasesCasarao != null)
            {
                flag1 = true;
                StartCoroutine(VerificarFasesCasarao.PassouFasePorao());
                VerificarFasesCasarao.AtualizarControladorFases();
            }
        }

        if (other.gameObject.CompareTag("cabeca") && !flag2)
        {
            flag2 = true;

            StartCoroutine(VerificarFasesCasarao.PassouFaseEntradaPrincipal());
            VerificarFasesCasarao.AtualizarControladorFases();
        }
        if (other.gameObject.CompareTag("parede") && !flag3)
        {
            if (VerificarFasesCasarao != null)
            {
                flag3 = true;

                StartCoroutine(VerificarFasesCasarao.PassouFaseSegundoAndar());
                VerificarFasesCasarao.AtualizarControladorFases();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("chave"))
        {
            if (completouSenha)
            {
                if (som != null)
                {
                    som.Play();
                }

                if (DebugChave != null)
                {
                    DebugChave.SetActive(true);
                }

                estaComChave = true;
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("porta"))
        {
            if (completouSenha && !abriu)
            {
                StartCoroutine(ColidiuPorta());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("porta"))
        {
            if (completouSenha && estaComChave)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(AbrirPorta());
                }
            }
        }
    }

    public IEnumerator ColidiuPorta()
    {
        if (debugPorta != null && debugPortaobj != null)
        {
            if (!estaComChave)
            {
                debugPorta.text = "Ache a chave para entrar";
            }
            else if (estaComChave)
            {
                debugPorta.text = "Clique 'F' para abrir";
            }

            debugPortaobj.SetActive(true);
            yield return new WaitForSeconds(3f);
            debugPortaobj.SetActive(false);
        }
    }

    public IEnumerator AbrirPorta()
    {
        if (somChave != null)
        {
            somChave.Play();
        }

        yield return new WaitForSeconds(1);

        if (porta != null)
        {
            porta.SetActive(true); // Ativa a porta
        }

        if (animatoPorta != null)
        {
            animatoPorta.SetTrigger("abrir");
        }

        abriu = true;
    }
}
