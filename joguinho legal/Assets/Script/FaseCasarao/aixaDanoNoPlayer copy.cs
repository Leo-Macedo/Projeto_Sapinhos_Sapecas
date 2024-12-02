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
            GameObject prefab = Resources.Load<GameObject>("Bombadedinehirobow");
            Instantiate(prefab, other.transform.position, Quaternion.identity);

            GameObject somDin = GameObject.FindWithTag("somdin");
            if (somDin != null)
            {
                AudioSource perseguirAudio = somDin.GetComponent<AudioSource>();
                if (perseguirAudio != null)
                {
                    perseguirAudio.Play();
                }
            }

            Destroy(gameObject);

            // Para a caixa ao colidir com o jogador
        }
        if (other.gameObject.CompareTag("outroobj") || other.gameObject.CompareTag("bomba"))
        {
            jaColidiu = true;
        }
    }
}
