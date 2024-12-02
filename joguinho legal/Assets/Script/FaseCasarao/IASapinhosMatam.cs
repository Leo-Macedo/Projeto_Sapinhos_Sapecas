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
    public bool podeAtacar = true;
    public Animator animatorPorta;

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
        SeguirEAnimar();
        AtacarCapanga();
        AtualizarListaCapangas(); // Atualiza a lista de capangas dinamicamente
        VerificarCapangasMortos(); // Verifica se todos os capangas estão mortos
    }

    private void SeguirEAnimar()
    {
        if (alvoAtual != null)
        {
            navMeshAgent.SetDestination(alvoAtual.position);
            animator.SetBool("correndo", navMeshAgent.velocity.sqrMagnitude > 0);
        }
    }

    private void AtacarCapanga()
    {
        if (alvoAtual == null)
        {
            SelecionarAlvo(); // Seleciona um novo alvo se não houver alvo atual
            return;
        }

        if (alvoAtual == null || alvoAtual.GetComponent<CapangaSegueEMorre>() == null)
        {
            Debug.Log("Alvo atual inválido ou destruído. Selecionando um novo alvo.");
            SelecionarAlvo();
            return;
        }

        CapangaSegueEMorre capangaScript = alvoAtual.GetComponent<CapangaSegueEMorre>();
        if (capangaScript != null && capangaScript.morreu)
        {
            Debug.Log("Alvo atual está morto. Selecionando um novo.");
            RemoveCapanga(alvoAtual); // Remove o capanga morto
            SelecionarAlvo(); // Seleciona outro capanga
            return;
        }

        float distancia = Vector3.Distance(transform.position, alvoAtual.position);
        if (distancia <= distAtaque && podeAtacar)
        {
            podeAtacar = false;
            animator.SetBool("soco", true);
            if (capangaScript != null)
            {
                capangaScript.ReceberDanoCapanga(1);
            }
            Invoke(nameof(VerificarCapangaMorto), 0.1f);
            Invoke(nameof(PermitirAtaque), 1);
            Invoke(nameof(ResetarAnimacaoSoco), 1);
        }
    }

    private void VerificarCapangaMorto()
    {
        if (alvoAtual != null)
        {
            CapangaSegueEMorre capangaScript = alvoAtual.GetComponent<CapangaSegueEMorre>();
            if (capangaScript != null && capangaScript.morreu)
            {
                Debug.Log("Capanga morto detectado: " + alvoAtual.name);
                RemoveCapanga(alvoAtual); // Remove o capanga da lista
                alvoAtual = null; // Limpa o alvo atual
                SelecionarAlvo(); // Seleciona um novo alvo
            }
        }
    }

    private void VerificarCapangasMortos()
    {
        bool todosMortos = true;

        foreach (Transform capanga in capangas)
        {
            CapangaSegueEMorre capangaScript = capanga.GetComponent<CapangaSegueEMorre>();
            if (capangaScript != null && !capangaScript.morreu)
            {
                todosMortos = false;
                break; // Interrompe a verificação se encontrar um capanga vivo
            }
        }

        if (todosMortos)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("correndo", false);
            Debug.Log("Todos os capangas estão mortos. NPC parou.");
            animatorPorta.SetTrigger("abrir");
        }
    }

    private void AtualizarListaCapangas()
    {
        GameObject[] capangasArray = GameObject.FindGameObjectsWithTag("capanga");
        capangas.Clear();

        foreach (GameObject capanga in capangasArray)
        {
            Transform capangaTransform = capanga.transform;
            if (capangaTransform != null) // Garante que o objeto não está destruído
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
            Transform capangaMaisProximo = null;
            float menorDistancia = Mathf.Infinity;

            foreach (Transform capangaTransform in capangas)
            {
                if (capangaTransform == null)
                    continue; // Ignorar capangas destruídos

                CapangaSegueEMorre capangaScript =
                    capangaTransform.GetComponent<CapangaSegueEMorre>();
                if (capangaScript != null && !capangaScript.morreu) // Apenas capangas vivos
                {
                    float distancia = Vector3.Distance(
                        transform.position,
                        capangaTransform.position
                    );
                    if (distancia < menorDistancia)
                    {
                        menorDistancia = distancia;
                        capangaMaisProximo = capangaTransform;
                    }
                }
            }

            alvoAtual = capangaMaisProximo;

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

    private void PermitirAtaque()
    {
        podeAtacar = true;
    }

    private void ResetarAnimacaoSoco()
    {
        animator.SetBool("soco", false);
    }
}
