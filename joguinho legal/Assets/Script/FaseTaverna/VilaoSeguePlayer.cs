using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    public Transform player; // Referência para o transform do jogador
    public float chargeForce = 10f; // Velocidade de investida
    public float chargeDuration = 2f; // Duração da investida
    private Vector3 direction; // Direção da investida
    public bool isCharging = false; // Flag para verificar se está na investida
    private Rigidbody rb; // Referência ao Rigidbody
    public float rotationSpeed = 2f;

    public VidaVilao vidaVilao;
    public VidaPersonagem vidaPersonagem;

    private bool PodeTomarDano = true;

    public GameObject particulaPrefab;
    public AudioSource audioSource;

    public GameObject portalvoltar;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody anexado ao vilão
        vidaVilao = GetComponent<VidaVilao>();

        // Modifica a vida diretamente (pode ser útil para cura ou outros efeitos)
        vidaVilao.Vida = 5;
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

        if(vidaVilao.Vida == 0)
        {
            portalvoltar.SetActive(true);
        }
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
        Invoke("PodeTomarDan", 5f);
    }

    private void IniciarInvestidaNovamente()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Debug.Log("Investida Retomada.");
        rb.WakeUp();
        isCharging = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("caixa"))
        {
            if (PodeTomarDano)
            {
                TomarDano(other);
            }
        }
        if (other.gameObject.CompareTag("parede"))
        {
            if (PodeTomarDano)
            {
                StopChargeNaParede();
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            vidaPersonagem.ReceberDano(1);
            StopChargeNaParede();
        }

        void TomarDano(Collision other)
        {
            vidaVilao.ReceberDanoVilao(1);
            StopChargeNaParede();
            Debug.Log("Colidiu com a parede. Investida parada.");
            PodeTomarDano = false;

            // Instancia a partícula na posição da parede
            Instantiate(particulaPrefab, other.transform.position, Quaternion.identity);

            // Destroi a parede
            Destroy(other.gameObject);
            audioSource.Play();
        }

        
    }

    private void PodeTomarDan()
    {
        PodeTomarDano = true;
    }
}
