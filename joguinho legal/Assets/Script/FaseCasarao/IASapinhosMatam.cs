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
        Seguiranimar();
        AtacarOCapanga();
        AtualizarListaCapangas(); // Atualiza a lista de capangas a cada quadro
        VerificarCapangasMortos(); // Verifica se todos os capangas estão mortos
    }

    private void Seguiranimar()
    {
        if (alvoAtual != null)
        {
            navMeshAgent.SetDestination(alvoAtual.position);
            animator.SetBool("correndo", navMeshAgent.velocity != Vector3.zero);
        }
    }

    private void AtacarOCapanga()
    {
        if (alvoAtual != null)
        {
            CapangaSegueEMorre capangaScript = alvoAtual.GetComponent<CapangaSegueEMorre>();
            if (capangaScript != null && capangaScript.morreu)
            {
                Debug.Log("Alvo atual está morto, selecionando um novo.");
                RemoveCapanga(alvoAtual); // Remove o capanga morto
                SelecionarAlvo();         // Seleciona outro capanga
                return;                   // Encerra o método para evitar erros
            }

            float distancia = Vector3.Distance(transform.position, alvoAtual.position);
            if (distancia <= distAtaque)
            {
                if (capangaScript != null && podeatacar)
                {
                    podeatacar = false;
                    animator.SetBool("soco", true);
                    capangaScript.ReceberDanoCapanga(1);
                    Invoke("VerificarCapangaMorto", 0.1f);
                    Invoke("PodeAtacar", 1);
                    Invoke("NãoPodeAtacar", 1);
                }
            }
        }
        else
        {
            SelecionarAlvo(); // Seleciona um novo alvo se não houver alvo atual
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
                alvoAtual = null;         // Limpa o alvo atual
                SelecionarAlvo();         // Seleciona um novo alvo
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
                break; // Não precisamos continuar se encontramos um capanga vivo
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
                CapangaSegueEMorre capangaScript = capangaTransform.GetComponent<CapangaSegueEMorre>();
                if (capangaScript != null && !capangaScript.morreu) // Apenas capangas vivos
                {
                    float distance = Vector3.Distance(transform.position, capangaTransform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestCapanga = capangaTransform;
                    }
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
