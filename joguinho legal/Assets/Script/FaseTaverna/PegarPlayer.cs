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
    private FicarInvisivel ficarInvisivel;


    private bool pegou;

    void Start()
    {
        ficarInvisivel = player.GetComponent<FicarInvisivel>();
        
        if (enemySpotlight != null)
        {
            enemySpotlight.color = originalColor;
        }
    }

    void Update()
    {
        if (!pegou)
        {

            // Verifica se o jogador est� dentro do alcance da vis�o do inimigo
            if (PlayerNaArea() && !ficarInvisivel.isInvisible)
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
        // Calcula a dire��o do inimigo para o jogador
        Vector3 directionToPlayer = player.GetComponent<Transform>().position - transform.position;

        // Verifica se o jogador est� dentro do cone de vis�o do Spot Light
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < enemySpotlight.spotAngle / 2 && directionToPlayer.magnitude <= detectionRange)
        {
            // Verifica se h� algum objeto na linha de vis�o que est� em uma layer que bloqueia a vis�o
            if (!Physics.Linecast(transform.position, player.GetComponent<Transform>().position, LayersBloqueadas))
            {
                return true; // Retorna verdadeiro se n�o houver bloqueio
            }
        }

        return false; // Retorna falso se o jogador estiver fora da vis�o ou bloqueado
    }
}
