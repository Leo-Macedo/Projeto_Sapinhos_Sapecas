using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimarSapos : MonoBehaviour
{
    private Animator animator; // Componente Animator para controlar animações
    private Vector3 ultimaPosicao; // Posição anterior do objeto
    private Vector3 movimento; // Movimento detectado do objeto

    void Start()
    {
        ultimaPosicao = transform.position; // Inicializa a última posição com a posição atual
        animator = GetComponent<Animator>(); // Obtém o componente Animator
    }

    void Update()
    {
        AnimacaoEAndando(); // Atualiza a animação com base no movimento
        farpar(); // Verifica se a tecla para "farpar" foi pressionada
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

    public void farpar()
    {
        // Verifica se a tecla E foi pressionada e aciona a animação de "farpar"
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("emote1"); // Aciona o gatilho "emote1" para a animação
        }
    }
}
