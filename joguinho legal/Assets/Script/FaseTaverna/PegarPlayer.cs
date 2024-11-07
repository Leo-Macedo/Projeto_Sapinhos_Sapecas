using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PegarPlayer : MonoBehaviour
{
    public Light enemySpotlight;
    public Color originalColor;
    public Color alertColor;
    public float detectionRange;
    public GameObject player;
    public LayerMask LayersBloqueadas;


    private bool pegou;

    void Start()
    {
        if (enemySpotlight != null)
        {
            enemySpotlight.color = originalColor;
        }
    }

    void Update()
    {
        if (!pegou)
        {

            // Verifica se o jogador está dentro do alcance da visão do inimigo
            if (PlayerNaArea())
            {
                StartCoroutine(perseguirPlayer());
                pegou = true;
            }
            else
            {
                // Volta a cor original do Spot Light
                enemySpotlight.color = originalColor;
            }
        }
        else
        {
            // Muda a cor do Spot Light para vermelho
            enemySpotlight.color = alertColor;
        }
    }

    IEnumerator perseguirPlayer()
    {
        gameObject.tag = "capanga";
        GetComponent<CapangaSegueEMorre>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<CapangaMovimentacao>().enabled = false;


        yield return new WaitForSeconds(0.5f);

    }

    

    bool PlayerNaArea()
    {
        // Calcula a direção do inimigo para o jogador
        Vector3 directionToPlayer = player.GetComponent<Transform>().position - transform.position;

        // Verifica se o jogador está dentro do cone de visão do Spot Light
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < enemySpotlight.spotAngle / 2 && directionToPlayer.magnitude <= detectionRange)
        {
            // Verifica se há algum objeto na linha de visão que está em uma layer que bloqueia a visão
            if (!Physics.Linecast(transform.position, player.GetComponent<Transform>().position, LayersBloqueadas))
            {
                return true; // Retorna verdadeiro se não houver bloqueio
            }
        }

        return false; // Retorna falso se o jogador estiver fora da visão ou bloqueado
    }
}
