using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    [Header("Referências")]
    public GameObject fumaca;
    public Transform player; // Referência para o transform do jogador
    public float chargeForce = 10f; // Força da investida
    public float chargeDuration = 2f; // Duração da investida
    private Vector3 direction; // Direção da investida
    public bool isCharging = false; // Flag para verificar se está na investida
    private Rigidbody rb; // Referência ao Rigidbody do vilão
    public float rotationSpeed = 2f; // Velocidade de rotação
    public GameObject fumacaoreia;

    public VidaVilao vidaVilao; // Referência ao script de vida do vilão
    public VidaPersonagem vidaPersonagem; // Referência ao script de vida do jogador
    public EscudoFuncionando escudoFuncionando;
    private bool podeTomarDano = true; // Flag para permitir ou não que o vilão tome dano

    public GameObject particulaPrefab; // Prefab de partículas para colisões
    public AudioSource audioSource; // Fonte de áudio para efeitos sonoros

    public GameObject portalvoltar; // Portal para voltar
    private bool podeDarDano = true;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody anexado ao vilão
        vidaVilao = GetComponent<VidaVilao>();
        animator = GetComponent<Animator>();
    }

    public void StartCharge()
    {
        if (player != null && rb != null)
        {
            // Define a direção da investida, ignorando o eixo Y para manter a aranha no plano XZ
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Mantém o movimento no plano XZ
            direction = directionToPlayer.normalized;
            fumaca.SetActive(true);
            // Aplica a força na direção do alvo
            rb.velocity = direction * chargeForce;
            isCharging = true;
            animator.SetTrigger("investir");
            Debug.Log("Investida iniciada. Direção: " + direction + " Força: " + chargeForce);
        }
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            // Move o vilão na direção da investida
            rb.velocity = direction * chargeForce;

            // Corrige a rotação para o eixo Z, com ajuste para alinhar a frente da aranha
            Quaternion targetRotation =
                Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Verifica se o vilão venceu
        Vitoria();
    }

private IEnumerator StopCharge()
{
    // Congela a posição do Rigidbody, mas não a rotação
    rb.constraints = RigidbodyConstraints.FreezePosition;
        fumaca.SetActive(false);

        // Desabilita as restrições de rotação para permitir a rotação manual
        rb.constraints &= ~RigidbodyConstraints.FreezeRotation;

    animator.SetTrigger("bater");
    Debug.Log("Investida parada.");

    yield return new WaitForSeconds(3);

    float lookDuration = 3f; // Duração de tempo que o vilão deve olhar para o jogador
    float elapsedTime = 0f;

    while (elapsedTime < lookDuration)
    {
        // Atualiza a rotação para olhar continuamente para o jogador
        if (player != null)
        {
            // Calcula a direção do vilão para o jogador
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Mantém no plano XZ

            // Cria a rotação de forma suave
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                fumacaoreia.SetActive(true);
            // Atualiza a rotação suavemente
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        elapsedTime += Time.deltaTime;
        yield return null; // Espera até o próximo quadro
    }

    // Retoma as restrições de física do Rigidbody
    rb.constraints = RigidbodyConstraints.None;
    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        fumacaoreia.SetActive(false);

        Debug.Log("Investida retomada.");
    rb.WakeUp();
    isCharging = false;
    podeTomarDano = true;
    podeDarDano = true;
}




    private void OnCollisionEnter(Collision other)
    {
        if (podeTomarDano)
        {
            if (other.gameObject.CompareTag("caixa"))
            {
                TomarDano(other);
            }
            else if (other.gameObject.CompareTag("parede"))
            {
                StartCoroutine(StopCharge());
            }
            else if (
                other.gameObject.CompareTag("Player")
                && !escudoFuncionando.escudoAtivo
                && podeDarDano
            )
            {
                vidaPersonagem.ReceberDano(1); // Aplica dano ao jogador
                podeDarDano = false;
                StartCoroutine(StopCharge());
            }
            else if (other.gameObject.CompareTag("Player") && escudoFuncionando.escudoAtivo)
            {
                StartCoroutine(StopCharge());
                escudoFuncionando.DesativarEscudo();
                GameObject prefab = Resources.Load<GameObject>("escudo");
                Instantiate(prefab, other.contacts[0].point, Quaternion.identity);
            }
        }
    }

    private void TomarDano(Collision other)
    {
        vidaVilao.ReceberDanoVilao(1); // Aplica dano ao vilão
        StartCoroutine(StopCharge());
        Debug.Log("Colidiu com a parede. Investida parada.");
        podeTomarDano = false;

        // Instancia a partícula na posição da parede
        Instantiate(particulaPrefab, other.transform.position, Quaternion.identity);

        // Destroi o objeto com a tag "caixa"
        Destroy(other.gameObject);
        audioSource.Play(); // Toca o som de colisão
        chargeForce += 1;

        Debug.Log("Agora a fforça é: " + chargeDuration);
    }

    private void Vitoria()
    {
        if (vidaVilao.Vida <= 0)
        {
            portalvoltar.SetActive(true); // Ativa o portal de volta
            PlayerPrefs.SetInt("TavernaCompletada", 1); // Marca a fase como completada
        }
    }
}
