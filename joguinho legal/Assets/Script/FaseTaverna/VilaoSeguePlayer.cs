using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    public Transform player;          // Referência para o transform do jogador
    public float chargeForce = 10f;   // Velocidade de investida
    public float chargeDuration = 2f; // Duração da investida
    private Vector3 targetPosition;   // Posição alvo para a investida
    public bool isCharging = false;  // Flag para verificar se está na investida
    private Rigidbody rb;             // Referência ao Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody anexado ao vilão
    }
    private void Update()
    {
        /*if (isCharging)
        {
            // Move o inimigo em direção à posição alvo
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
            }
            else
            {
                // Para a investida quando atinge o alvo
                isCharging = false;
                // Execute outras ações aqui, se necessário (e.g., aplicar dano)
            }
        }*/
    }

    // Método para iniciar a investida
    public void StartCharge()
    {
         if (player != null && rb != null )
        {

          // Define a posição alvo como a posição atual do jogador
            targetPosition = player.position;
            // Calcula a direção da investida
            Vector3 direction = (targetPosition - transform.position).normalized;
            // Aplica a força na direção do alvo
            rb.AddForce(direction * chargeForce, ForceMode.Impulse);
            isCharging = true;
             Debug.Log("Investida iniciada. Direção: " + direction + " Força: " + chargeForce);

        }
    }

    private void StopCharge()
    {
          isCharging = false;
        // Opcionalmente, você pode zerar a velocidade do Rigidbody aqui
        rb.velocity = Vector3.zero;
        Debug.Log("Investida parada.");
    }
private void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("parede"))
    {
        StopCharge();
           Debug.Log("Colidiu com a parede. Investida parada.");
    }
}
}

