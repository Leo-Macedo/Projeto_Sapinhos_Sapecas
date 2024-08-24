using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilaoOlhaPlayer : MonoBehaviour
{
      public VilaoSeguePlayer vilaoSeguePlayer; // Referência ao script de investida
    public float detectionRange = 10f;     // Alcance de detecção do jogador
    private VidaVilao vidaVilao;

    private void Start()
    {
      vidaVilao = GetComponent<VidaVilao>();
    }
    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, vilaoSeguePlayer.player.position);
        if (distanceToPlayer < detectionRange && !vilaoSeguePlayer.isCharging && vidaVilao.Vida > 0)
        {
            Debug.Log("Olhou Investida" + vilaoSeguePlayer.isCharging);
            // Inicia a investida
            vilaoSeguePlayer.StartCharge();
        }
    }

}
