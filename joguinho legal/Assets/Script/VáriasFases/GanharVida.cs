using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanharVida : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VidaPersonagem vidaPersonagem = other.gameObject.GetComponent<VidaPersonagem>();

            if (vidaPersonagem == null)
            {
                // Não faz nada se o componente VidaPersonagem não existir
                return;
            }

            // Verifica se a vida atual é menor ou igual a 2 antes de realizar as ações
            if (vidaPersonagem.vidaAtual <= 2)
            {
                // Incrementa a vida
                vidaPersonagem.vidaAtual += 1;

                // Instancia o prefab de cura
                GameObject prefab = Resources.Load<GameObject>("cura");
                if (prefab != null)
                {
                    Instantiate(prefab, transform.position, Quaternion.identity);
                    Debug.Log("Prefab instanciado com sucesso!");
                }
                else
                {
                    Debug.LogWarning("Prefab 'cura' não encontrado em Resources!");
                }

                // Toca o som de cura, se disponível
                GameObject sireneObj = GameObject.FindWithTag("curasom");
                if (sireneObj != null)
                {
                    AudioSource sireneAudio = sireneObj.GetComponent<AudioSource>();
                    if (sireneAudio != null)
                    {
                        sireneAudio.Play();
                    }
                }

                // Destroi o objeto atual
                Destroy(gameObject);

                // Destroi o objeto pai, se existir
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
