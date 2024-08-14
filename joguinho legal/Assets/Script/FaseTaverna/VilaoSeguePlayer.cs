using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoSeguePlayer : MonoBehaviour
{
    private Animator animator;
    private Vector3 ultimaPosicao;
    private Vector3 movimento;

    public Transform player; // Referência ao transform do jogador
    public float speed = 5f; // Velocidade de movimento do vilão
    public float detectionRange = 10f; // Distância de detecção do jogador
    public float zMinLimit = -4f; // Limite inferior do movimento no eixo Z
    public float zMaxLimit = 4f; // Limite superior do movimento no eixo Z
    private bool isChasing = false; // Flag para verificar se o vilão está correndo em linha reta

    private Rigidbody rb;

    void Start()
    {
        ultimaPosicao = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //AnimacaoEAndando();

        // Se o vilão está perseguindo o jogador
        if (isChasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Seguir o jogador e iniciar a perseguição
        SeguirEBater();
         }
    }

    private void AnimacaoEAndando()
    {
        movimento = transform.position - ultimaPosicao;
        ultimaPosicao = transform.position;

        if (movimento != Vector3.zero)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
        }
    }

    public void SeguirEBater()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    Debug.Log("Distância para o jogador: " + distanceToPlayer);

    if (distanceToPlayer < detectionRange)
    {
        Debug.Log("Jogador detectado, iniciando perseguição");
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        rb.MovePosition(newPosition);
        isChasing = true;
    }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Se o vilão colidir com uma parede ou obstáculo, ele para
        if (other.gameObject.CompareTag("parede"))
        {
            Debug.Log("Não está andando");
            rb.velocity = Vector3.zero;
            isChasing = false;
        }
    }
}

