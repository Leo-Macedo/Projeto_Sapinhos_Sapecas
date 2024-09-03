using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    [Header("Referências")]
    public Transform player; // Referência para o transform do jogador
    public float chargeForce = 10f; // Força da investida
    public float chargeDuration = 2f; // Duração da investida
    private Vector3 direction; // Direção da investida
    public bool isCharging = false; // Flag para verificar se está na investida
    private Rigidbody rb; // Referência ao Rigidbody do vilão
    public float rotationSpeed = 2f; // Velocidade de rotação

    public VidaVilao vidaVilao; // Referência ao script de vida do vilão
    public VidaPersonagem vidaPersonagem; // Referência ao script de vida do jogador

    private bool podeTomarDano = true; // Flag para permitir ou não que o vilão tome dano

    public GameObject particulaPrefab; // Prefab de partículas para colisões
    public AudioSource audioSource; // Fonte de áudio para efeitos sonoros

    public GameObject portalvoltar; // Portal para voltar

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody anexado ao vilão
        vidaVilao = GetComponent<VidaVilao>();

        
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            // Move o vilão na direção da investida
            rb.velocity = direction * chargeForce;

            // Rotaciona suavemente para a direção da investida
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Verifica se o vilão venceu
        Vitoria();
    }

    // Método para iniciar a investida
    public void StartCharge()
    {
        if (player != null && rb != null)
        {
            // Define a direção da investida
            direction = (player.position - transform.position).normalized;
            // Aplica a força na direção do alvo
            rb.velocity = direction * chargeForce;
            isCharging = true;
            Debug.Log("Investida iniciada. Direção: " + direction + " Força: " + chargeForce);
        }
    }

    private void StopChargeNaParede()
    {
        // Congela o movimento do Rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition;
        Debug.Log("Investida parada.");
        Invoke("IniciarInvestidaNovamente", 3.2f);
        Invoke("PodeTomarDano", 5f);
    }

    private void IniciarInvestidaNovamente()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Debug.Log("Investida retomada.");
        rb.WakeUp();
        isCharging = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("caixa"))
        {
            if (podeTomarDano)
            {
                TomarDano(other);
            }
        }

        if (other.gameObject.CompareTag("parede"))
        {
            if (podeTomarDano)
            {
                StopChargeNaParede();
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            vidaPersonagem.ReceberDano(1); // Aplica dano ao jogador
            StopChargeNaParede();
        }
    }

    private void TomarDano(Collision other)
    {
        vidaVilao.ReceberDanoVilao(1); // Aplica dano ao vilão
        StopChargeNaParede();
        Debug.Log("Colidiu com a parede. Investida parada.");
        podeTomarDano = false;

        // Instancia a partícula na posição da parede
        Instantiate(particulaPrefab, other.transform.position, Quaternion.identity);

        // Destroi o objeto com a tag "caixa"
        Destroy(other.gameObject);
        audioSource.Play(); // Toca o som de colisão
    }

    private void PodeTomarDano()
    {
        podeTomarDano = true;
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
