using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoOlhaPlayer : MonoBehaviour
{
    [Header("Referências")]
    public VilaoSeguePlayer vilaoSeguePlayer; // Referência ao script que controla o movimento do vilão
    public float detectionRange = 10f; // Alcance de detecção do jogador

    private VidaVilao vidaVilao; // Referência ao script de vida do vilão

    void Start()
    {
        // Inicializa a referência ao script de vida do vilão
        vidaVilao = GetComponent<VidaVilao>();
    }

    void Update()
    {
        // Calcula a distância entre o vilão e o jogador
        float distanceToPlayer = Vector3.Distance(transform.position, vilaoSeguePlayer.player.position);

        // Verifica se o jogador está dentro do alcance de detecção, se o vilão não está carregando uma investida e se o vilão ainda tem vida
        if (distanceToPlayer < detectionRange && !vilaoSeguePlayer.isCharging && vidaVilao.Vida > 0)
        {
            Debug.Log("Olhou Investida: " + vilaoSeguePlayer.isCharging); // Exibe no console se o vilão está olhando para o jogador e não está carregando
            // Inicia a investida
            vilaoSeguePlayer.StartCharge();
        }
    }
}
