using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    public Transform player;          // Referência para o transform do jogador
    public float chargeForce = 10f;   // Velocidade de investida
    public float chargeDuration = 2f; // Duração da investida
    private Vector3 direction;        // Direção da investida
    public bool isCharging = false;   // Flag para verificar se está na investida
    private Rigidbody rb;             // Referência ao Rigidbody
    public float rotationSpeed = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody anexado ao vilão
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            // Move o vilão na direção da investida
            rb.velocity = direction * chargeForce;

            // Rotaciona suavemente para a direção da investida
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

 
    private void StopCharge()
    {
        // Zera a velocidade do Rigidbody
        rb.velocity = Vector3.zero;
        Debug.Log("Investida parada.");
        Invoke("IniciarInvestidaNovamente", 3.2f);
    }

    private void IniciarInvestidaNovamente()
    {
        isCharging = false;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("parede"))
        {
            StopCharge();
            Debug.Log("Colidiu com a parede. Investida parada.");
        }
    }
}
