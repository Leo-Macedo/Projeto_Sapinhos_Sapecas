using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Para usar NavMeshAgent

public class MeloMovimentacao : MonoBehaviour
{
    public Transform player; // Jogador a seguir
    public float alturaVoo = 5f; // Altura de voo da mosca
    public float tempoPousado = 3f; // Tempo que a mosca fica pousada
    public float tempoVoando = 5f; // Tempo que a mosca fica voando

    public bool voando = true; // Define se a mosca está voando
    private bool pousado = false; // Define se a mosca está pousada
    public bool podeReceberDano = false;
    private Rigidbody rb;
    private NavMeshAgent agent; // Referência ao NavMeshAgent

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        rb.useGravity = false; // Desativa a gravidade ao começar voando
        agent.enabled = true; // Ativa o NavMeshAgent para começar a seguir o player
        StartCoroutine(CicloMosca());
    }

    IEnumerator CicloMosca()
    {
        while (true)
        {
            if (voando)
            {
                // A mosca voa por um período de tempo
                float tempoVooRestante = tempoVoando;
                while (tempoVooRestante > 0f)
                {
                    SeguirPlayer(); // Segue o jogador enquanto estiver voando
                    tempoVooRestante -= Time.deltaTime;
                    yield return null;
                }
                
                // Quando o tempo de voo acaba, ela pousa
                Pousar();
            }
            else if (pousado)
            {
                // A mosca espera pousada por um período de tempo antes de voltar a voar
                yield return new WaitForSeconds(tempoPousado);
                yield return StartCoroutine(VoltarAVoarSuave()); // Volta a voar suavemente
            }
        }
    }

    void SeguirPlayer()
    {
        if (player != null && agent.enabled)
        {
            // Define o destino do NavMeshAgent como a posição do jogador no plano XZ (ignorando a altura)
            agent.SetDestination(player.position);

            // Mantém a altura da mosca ajustada à altura de voo desejada
            Vector3 posicaoComAltura = new Vector3(transform.position.x, alturaVoo, transform.position.z);
            transform.position = posicaoComAltura;
        }
    }

    public void Pousar()
    {
        voando = false;
        pousado = true;
        podeReceberDano = true;
        agent.enabled = false; // Desativa o NavMeshAgent ao pousar
        rb.useGravity = true; // Ativa a gravidade ao pousar
        rb.velocity = Vector3.zero; // Para o movimento da mosca
    }

    IEnumerator VoltarAVoarSuave()
    {
        voando = true;
        pousado = false;
        podeReceberDano = false;
        rb.useGravity = false; // Desativa a gravidade ao voltar a voar
        agent.enabled = true; // Ativa novamente o NavMeshAgent

        // Transição suave de subida
        float alturaInicial = transform.position.y;
        float tempoInicio = Time.time;
        float duracao = 1f; // Duração da transição suave

        while (Time.time - tempoInicio < duracao)
        {
            float t = (Time.time - tempoInicio) / duracao;
            Vector3 novaPosicao = new Vector3(transform.position.x, Mathf.Lerp(alturaInicial, alturaVoo, t), transform.position.z);
            transform.position = novaPosicao; // Controla a altura durante a subida
            yield return null;
        }

        // Garante que a altura final seja alcançada
        transform.position = new Vector3(transform.position.x, alturaVoo, transform.position.z);
    }

}
