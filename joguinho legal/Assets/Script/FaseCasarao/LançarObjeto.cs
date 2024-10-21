using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LançarObjeto : MonoBehaviour
{
    public GameObject caixaPrefab; // O prefab da caixa
    public Transform spawnPoint; // Ponto onde a caixa irá aparecer
    public float throwForce = 10f; // Força do lançamento
    public float rotationSpeed = 5f; // Velocidade de rotação
    private Animator animator; // Referência ao Animator
    private GameObject caixa; // Referência à caixa atual
    private Transform player; // Referência ao transform do jogador

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Obtém a posição do jogador
        StartCoroutine(LançarCaixaPeriodicamente()); // Inicia a rotina de lançamento
    }

    private IEnumerator LançarCaixaPeriodicamente()
    {
        while (true) // Loop infinito
        {
            if (caixa == null) // Verifica se não há uma caixa sendo lançada
            {
                // Ativa a animação de lançar
                animator.SetTrigger("lançar");

                // Inicia a rotação enquanto a animação estiver em execução
                yield return StartCoroutine(RotacionarDuranteAnimacao());

                
            }
            yield return new WaitForSeconds(2f); // Espera 3 segundos antes de repetir
        }
    }

    private IEnumerator RotacionarDuranteAnimacao()
    {
        float tempoAnimacao = animator.GetCurrentAnimatorStateInfo(0).length;
        float tempoDecorrido = 0f;

        while (tempoDecorrido < tempoAnimacao)
        {
            RotacionarParaJogador();
            tempoDecorrido += Time.deltaTime;
            yield return null; // Espera um quadro
        }
    }

    private void RotacionarParaJogador()
    {
        if (player != null)
        {
            // Calcula a direção para o jogador
            Vector3 direcao = (player.position - transform.position).normalized;

            // Calcula a rotação desejada
            Quaternion lookRotation = Quaternion.LookRotation(direcao);

            // Rotaciona suavemente em direção ao jogador
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Método a ser chamado pelo evento da animação
    public void LançarCaixa()
    {
        // Instancia a caixa na posição do ponto de spawn
        caixa = Instantiate(caixaPrefab, spawnPoint.position, spawnPoint.rotation);

        // Adiciona força para lançar a caixa em direção ao jogador
        Rigidbody rb = caixa.GetComponent<Rigidbody>();
        Vector3 direcaoLançamento = (player.position - spawnPoint.position).normalized; // Direção para o jogador
        rb.AddForce(direcaoLançamento * throwForce, ForceMode.Impulse);

        // Reseta a referência à caixa
        caixa = null;
    }
}
