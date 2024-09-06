using System.Collections;
using UnityEngine;

public class MeloMovimentacao : MonoBehaviour
{
    public Transform[] waypoints; // Array de waypoints a seguir
    public float velocidadeVoo = 5f; // Velocidade de voo da mosca
    public float tempoPousado = 3f; // Tempo que a mosca fica pousada
    public float tempoVoando = 5f; // Tempo que a mosca fica voando
    public float velocidadeSubida = 2f; // Velocidade para subir ou descer suavemente

    private int waypointAtual = 0; // Índice do waypoint atual
    private bool voando = true; // Define se a mosca está voando
    private bool pousado = false; // Define se a mosca está pousada
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Começar voando, então gravidade desligada
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
                    VoarParaWaypoints(); // Movimentação da mosca entre os waypoints
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

    void VoarParaWaypoints()
    {
        if (waypoints.Length == 0) return;

        // Pega o próximo waypoint
        Transform waypoint = waypoints[waypointAtual];
        
        // Calcula a direção para o waypoint (incluindo a altura do waypoint)
        Vector3 direcao = (waypoint.position - transform.position).normalized;

        // Move a mosca na direção do waypoint
        Vector3 novaPosicao = transform.position + direcao * velocidadeVoo * Time.deltaTime;

        // Ajusta a altura de voo para a altura do waypoint
        novaPosicao.y = waypoint.position.y;

        rb.MovePosition(novaPosicao);

        // Checa se a mosca chegou perto o suficiente do waypoint
        if (Vector3.Distance(transform.position, waypoint.position) < 0.2f)
        {
            waypointAtual++;
            if (waypointAtual >= waypoints.Length)
            {
                // Se chegar no último waypoint, volta para o primeiro
                waypointAtual = 0;
            }
        }
    }

    public void Pousar()
    {
        voando = false;
        pousado = true;
        rb.useGravity = true; // Ativa a gravidade ao pousar
        rb.velocity = Vector3.zero; // Para o movimento da mosca
    }

    IEnumerator VoltarAVoarSuave()
    {
        voando = true;
        pousado = false;
        rb.useGravity = false; // Desativa a gravidade ao voltar a voar

        // Define a altura do waypoint atual
        float alturaWaypoint = waypoints[waypointAtual].position.y;
        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoFinal = new Vector3(transform.position.x, alturaWaypoint, transform.position.z);

        float tempoInicio = Time.time;
        float duracao = 1f; // Duração da transição suave
        while (Time.time - tempoInicio < duracao)
        {
            float t = (Time.time - tempoInicio) / duracao;
            Vector3 novaPosicao = Vector3.Lerp(posicaoInicial, posicaoFinal, t);
            rb.MovePosition(novaPosicao);
            yield return null;
        }
        rb.MovePosition(posicaoFinal); // Garante que a posição final seja alcançada
    }
}
