using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaDanoNoPlayer : MonoBehaviour
{
    public bool jaColidiu = false; // Flag para verificar se a colisão foi com o jogador
    private GameObject player;
    private VidaPersonagem vidaPersonagem;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vidaPersonagem = player.GetComponent<VidaPersonagem>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Se a colisão for com o jogador e ainda não tiver colidido antes
        if (other.gameObject.CompareTag("Player") && !jaColidiu)
        {
            vidaPersonagem.ReceberDano(1);
            jaColidiu = true;

            // Para a caixa ao colidir com o jogador
        }
        if (other.gameObject.CompareTag("outroobj"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            jaColidiu = true;
        }
    }
}
