using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AçõesPersonagem : MonoBehaviour
{
    [Header("Referências e Configurações")]
    private Animator animator; // Animator do personagem
    private bool estadesviando = false;
    private CapangaSegueEMorre capangaSegueEMorre;
    private bool podeMatarCapanga = false;
    public float distAtaque = 1f; // Distância máxima para atacar o capanga
    private bool cursorAtivo = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no objeto.");
        }
    }

    void Update()
    {
       HabilitarCursor();
        // Entrada do jogador para ações
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("soco");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("chute");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("emote1");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            IniciarDesvio();
        }
    }

    private void HabilitarCursor()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorAtivo = !cursorAtivo; // Alterna o estado do cursor
            Cursor.lockState = cursorAtivo ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = cursorAtivo;
        }
    }

    void IniciarDesvio()
    {
        estadesviando = true;
        animator.SetTrigger("desvio");
        StartCoroutine(PararDesvioDepoisTempo(2f)); // Supondo que o desvio dura 2 segundos
    }

    private IEnumerator PararDesvioDepoisTempo(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        estadesviando = false;
    }

    public bool EstaDesviando()
    {
        return estadesviando;
    }

    public void DarDanoCapanga()
    {
        EncontrarCapangaMaisProximo();
        if (capangaSegueEMorre != null)
        {
            float distancia = Vector3.Distance(transform.position, capangaSegueEMorre.transform.position);
            if (podeMatarCapanga && distancia <= distAtaque)
            {
                capangaSegueEMorre.VericarNocauteCapanga();
                podeMatarCapanga = false;
            }
        }
    }

    public void ResetarPodeMatarCapanga()
    {
        podeMatarCapanga = true;
    }

    private void EncontrarCapangaMaisProximo()
    {
        GameObject[] capangas = GameObject.FindGameObjectsWithTag("capanga");
        Transform capangaTransformMaisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject capanga in capangas)
        {
            float distancia = Vector3.Distance(transform.position, capanga.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                capangaTransformMaisProximo = capanga.transform;
            }
        }

        if (capangaTransformMaisProximo != null)
        {
            capangaSegueEMorre = capangaTransformMaisProximo.GetComponent<CapangaSegueEMorre>();
        }
    }
}
