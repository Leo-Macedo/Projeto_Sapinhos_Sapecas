using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeloMovimentacao : MonoBehaviour
{
    public Rigidbody playerRb;
    public float raio;
    public float forca;

    public AudioSource queda;
    public AudioSource somVoando;
    public Transform player; // Jogador a seguir
    public float alturaVoo = 5f; // Altura de voo da mosca
    public float tempoPousado = 3f; // Tempo que a mosca fica pousada
    public float tempoVoando = 5f; // Tempo que a mosca fica voando
    public Transform ponto;
    private bool jaColidiu;

    public bool voando = true; // Define se a mosca está voando
    private bool pousado = false; // Define se a mosca está pousada
    public bool podeReceberDano = false;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator animator;
    private VidaVilao vidaVilao;
    private float tempoVooRestante;
    private float tempoPousoRestante;
    private bool pousando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        vidaVilao = GetComponent<VidaVilao>();

        rb.useGravity = false; // Desativa a gravidade ao começar voando
        agent.enabled = true; // Ativa o NavMeshAgent para começar a seguir o player
        tempoVooRestante = tempoVoando; // Inicializa o tempo de voo restante
        tempoPousoRestante = tempoPousado; // Inicializa o tempo de pouso restante
        StartCoroutine(CicloMosca());
    }

    void Update()
    {
        animator.SetBool("voando", voando);
    }

    IEnumerator CicloMosca()
    {
        while (!vidaVilao.morreuvilao)
        {
            if (voando)
            {
                // A mosca voa até que o tempo de voo restante acabe
                while (tempoVooRestante > 0f)
                {
                    SeguirPlayer();
                    tempoVooRestante -= Time.deltaTime;
                    yield return null;
                }

                if (!pousando) // Verifica se já está pousando
                {
                    yield return StartCoroutine(Pousar()); // Aguarda a conclusão da corrotina
                }
            }
            else if (pousado)
            {
                if (tempoPousoRestante > 0f)
                {
                    tempoPousoRestante -= Time.deltaTime;
                }
                else
                {
                    tempoPousoRestante = tempoPousado; // Reseta o tempo de pouso para o valor inicial
                    yield return StartCoroutine(VoltarAVoarSuave()); // Volta a voar suavemente
                }

                yield return null;
            }
        }
    }

    void SeguirPlayer()
    {
        if (player != null && agent.enabled)
        {
            agent.SetDestination(player.position);

            Vector3 posicaoComAltura = new Vector3(
                transform.position.x,
                alturaVoo,
                transform.position.z
            );
            somVoando.Play();
            transform.position = posicaoComAltura;
        }
    }

    public IEnumerator Pousar()
    {
        somVoando.Stop();
        animator.SetTrigger("cair");
        pousando = true;
        yield return new WaitForSeconds(1f);
        voando = false;
        pousado = true;
        podeReceberDano = true;
        agent.enabled = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        tempoPousoRestante = tempoPousado;
        pousando = false;
    }

    public void TomarDano()
    {
        // Zera o tempo de pouso, forçando a mosca a voltar a voar imediatamente
        tempoPousoRestante = 0f;
    }

    IEnumerator VoltarAVoarSuave()
    {
        animator.SetTrigger("levantar");

        voando = true;
        pousado = false;
        podeReceberDano = false;
        rb.useGravity = false;
        agent.enabled = true;
        tempoVooRestante = tempoVoando; // Reinicia o tempo de voo

        float alturaInicial = transform.position.y;
        float tempoInicio = Time.time;
        float duracao = 1f;
        jaColidiu = false;

        while (Time.time - tempoInicio < duracao)
        {
            float t = (Time.time - tempoInicio) / duracao;
            Vector3 novaPosicao = new Vector3(
                transform.position.x,
                Mathf.Lerp(alturaInicial, alturaVoo, t),
                transform.position.z
            );
            transform.position = novaPosicao;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, alturaVoo, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao") && (pousando || pousado) && !jaColidiu)
        {
            GameObject prefab = Resources.Load<GameObject>("Chao");
            Instantiate(prefab, ponto.position, ponto.rotation);
            queda.Play();
            jaColidiu = true;

            if (Vector3.Distance(transform.position, player.position) <= raio)
            {
                playerRb.AddForce(Vector3.up * forca, ForceMode.Impulse);
            }
        }
    }
}
