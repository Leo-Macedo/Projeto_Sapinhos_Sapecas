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

            if (vidaPersonagem != null && vidaPersonagem.vidaAtual <= 2)
            {
                vidaPersonagem.vidaAtual += 1;
                GameObject prefab = Resources.Load<GameObject>("cura");

                Instantiate(prefab, transform.position, Quaternion.identity);
                Debug.Log("Prefab instanciado com sucesso!");

                GameObject sireneObj = GameObject.FindWithTag("curasom");
                if (sireneObj != null)
                {
                    AudioSource sireneAudio = sireneObj.GetComponent<AudioSource>();
                    if (sireneAudio != null)
                    {
                        sireneAudio.Play();
                    }
                }
                Destroy(gameObject);

               
                    Destroy(transform.parent.gameObject);
                   



            }
            else
            {
                Debug.LogWarning("Componente VidaPersonagem não encontrado no jogador!");
            }
        }
    }

}
