using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IASapinhosMatam : MonoBehaviour
{
    // Referências
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public float distAtaque;
    public bool podeatacar = true;

    private Transform alvoAtual; // Capanga que o NPC está seguindo
    private List<Transform> capangas = new List<Transform>(); // Lista de capangas

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Inicializa a lista de capangas e seleciona o alvo inicial
        AtualizarListaCapangas();
        SelecionarAlvo();
    }

    void Update()
    {
        Seguiranimar();
        AtacarOCapanga();
    }

    private void Seguiranimar()
    {
        if (alvoAtual != null)
        {
            navMeshAgent.SetDestination(alvoAtual.position);

            // Animações de andar e parar
            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetBool("andando", true);
            }
            else
            {
                animator.SetBool("andando", false);
            }
        }
    }

    private void AtacarOCapanga()
    {
        if (alvoAtual != null)
        {
            // Atacar o capanga
            float distancia = Vector3.Distance(transform.position, alvoAtual.position);
            if (distancia <= distAtaque)
            {
                if (podeatacar)
                {
                    podeatacar = false;
                    animator.SetBool("soco", true);

                    Animator animatorCapanga = alvoAtual.GetComponent<Animator>();
                    NavMeshAgent navMeshAgentCapanga = alvoAtual.GetComponent<NavMeshAgent>();

                    animatorCapanga.SetBool("nocaute", true);
                    navMeshAgentCapanga.speed = 0f;
                    navMeshAgentCapanga.isStopped = true;

                    // Remove o capanga nocauteado da lista e seleciona um novo alvo
                    RemoveCapanga(alvoAtual);
                    Invoke("SelecionarAlvo", 0.1f); // Chama SelecionarAlvo depois de um curto intervalo

                    Invoke("PodeAtacar", 1);
                    Invoke("NãoPodeAtacar", 1);
                }
            }
        }
    }

    private void AtualizarListaCapangas()
    {
        GameObject[] capangasArray = GameObject.FindGameObjectsWithTag("capanga");
        capangas.Clear();

        foreach (GameObject capanga in capangasArray)
        {
            Transform capangaTransform = capanga.GetComponent<Transform>();
            if (capangaTransform != null && !capangas.Contains(capangaTransform))
            {
                capangas.Add(capangaTransform);
            }
        }

        Debug.Log("Lista de capangas atualizada. Total de capangas: " + capangas.Count);
    }

    private void SelecionarAlvo()
    {
        if (capangas.Count > 0)
        {
            Transform closestCapanga = null;
            float minDistance = Mathf.Infinity;

            foreach (Transform capangaTransform in capangas)
            {
                float distance = Vector3.Distance(transform.position, capangaTransform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCapanga = capangaTransform;
                }
            }

            alvoAtual = closestCapanga;

            if (alvoAtual != null)
            {
                Debug.Log("Novo alvo selecionado: " + alvoAtual.name);
            }
            else
            {
                Debug.Log("Nenhum capanga restante para seguir.");
            }
        }
        else
        {
            alvoAtual = null;
            Debug.Log("Nenhum capanga na lista.");
        }
    }

    private void RemoveCapanga(Transform capanga)
    {
        if (capangas.Contains(capanga))
        {
            capangas.Remove(capanga);
            Debug.Log("Capanga removido: " + capanga.name);
        }
    }

    public void NãoPodeAtacar()
    {
        animator.SetBool("soco", false);
    }

    public void PodeAtacar()
    {
        podeatacar = true;
    }
}
