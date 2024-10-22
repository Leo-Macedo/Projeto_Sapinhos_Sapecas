using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaNoGol : MonoBehaviour
{
    private InstanciadorDeBolas instanciador;
     // Referência ao script de instanciamento

    void Start()
    {
        instanciador = GameObject.FindWithTag("instanciador").GetComponent<InstanciadorDeBolas>();
    }

    // Verifica a colisão com diferentes objetos para destruir a bola e permitir nova instância
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gol"))
        {
            instanciador.SomarGols();
            Debug.Log("GOOOOOLLLLL");
            Destroy(gameObject);
            instanciador.podeInstanciar = true; // Permite instanciar uma nova bola
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("naogol"))
        {
            Debug.Log("Pra fora!");
            Destroy(gameObject);
            instanciador.podeInstanciar = true; // Permite instanciar uma nova bola
        }
        else if (other.gameObject.CompareTag("trave"))
        {
            Debug.Log("Na trave!");
            Destroy(gameObject);
            instanciador.podeInstanciar = true; // Permite instanciar uma nova bola
        }
        else if (other.gameObject.CompareTag("goleiro"))
        {
            Debug.Log("Defendeu goleiro!");
            Destroy(gameObject);
            instanciador.podeInstanciar = true; // Permite instanciar uma nova bola
        }
    }
}
