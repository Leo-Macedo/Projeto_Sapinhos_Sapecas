using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimarSapos : MonoBehaviour
{
    private Animator animator; // Componente Animator para controlar animações
    private Vector3 ultimaPosicao; // Posição anterior do objeto
    private Vector3 movimento; // Movimento detectado do objeto

    [SerializeField]
    private float groundCheckSize; // Tamanho da esfera de verificação de chão

    [SerializeField]
    private Vector3 groundCheckPosition; // Posição relativa da verificação de chão

    public bool isGrounded; // Indica se o personagem está no chão

    [SerializeField]
    private LayerMask layermask; // Máscara de camada para detectar o chão

    void Start()
    {
        ultimaPosicao = transform.position; // Inicializa a última posição com a posição atual
        animator = GetComponent<Animator>(); // Obtém o componente Animator
    }

    void Update()
    {
        AnimacaoEAndando(); // Atualiza a animação com base no movimento
        // Obtém a escala atual do objeto
        Vector3 escalaAtual = transform.localScale;

        // Calcula o tamanho e a posição da verificação de chão com base na escala do objeto
        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);
        // Realiza a verificação de chão com os valores escalados
        var groundcheck = Physics.OverlapSphere(
            transform.position + posicaoVerificada,
            tamanhoVerificado,
            layermask
        );
        isGrounded = groundcheck.Length != 0;
        animator.SetBool("pulo", !isGrounded);
    }

    private void AnimacaoEAndando()
    {
        movimento = transform.position - ultimaPosicao; // Calcula o movimento do objeto

        ultimaPosicao = transform.position; // Atualiza a última posição

        // Verifica se o objeto está se movendo e ajusta a animação
        if (movimento != Vector3.zero)
        {
            animator.SetBool("andando", true); // Define o parâmetro "andando" como verdadeiro
        }
        else
        {
            animator.SetBool("andando", false); // Define o parâmetro "andando" como falso
        }
    }

    private void OnDrawGizmos()
    {
        // Obtém a escala atual do objeto
        Vector3 escalaAtual = transform.localScale;

        // Calcula o tamanho e a posição da verificação de chão com base na escala do objeto
        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + posicaoVerificada, tamanhoVerificado);
    }
}
