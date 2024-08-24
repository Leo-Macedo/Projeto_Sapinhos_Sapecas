using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AçõesPersonagem : MonoBehaviour
{
    //Animações de Ação do personagem
    private Animator animator;
    private bool estadesviando = false;
    private CapangaSegueEMorre capangaSegueEMorre;
    private bool podeMatarCapanga = false;
    public float distAtaque; // Distância máxima para atacar o capanga

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

    //Funções vazias
    public void DarDanoNoVilao() { }

    public void AcionarSoco() { }

    public void DarDanoCapanga()
    {
        EncontrarCapangaMaisProximo();
        if (capangaSegueEMorre != null)
        {
            float distancia = Vector3.Distance(
                transform.position,
                capangaSegueEMorre.transform.position
            );

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
