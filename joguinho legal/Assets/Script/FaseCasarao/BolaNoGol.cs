using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaNoGol : MonoBehaviour
{
    private InstanciadorDeBolas instanciador;
    private bool jaColidiu;

    // Referência ao script de instanciamento

    void Start()
    {
        instanciador = GameObject.FindWithTag("instanciador").GetComponent<InstanciadorDeBolas>();
    }

    // Verifica a colisão com diferentes objetos para destruir a bola e permitir nova instância
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gol") && !jaColidiu)
        {
            instanciador.SomarGols();
            jaColidiu = true;
            StartCoroutine(ExplodirBolaGol());
            instanciador.StartCoroutine(instanciador.DebugGol(0));

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("naogol") && !jaColidiu)
        {
            StartCoroutine(ExplodirBola());
            instanciador.StartCoroutine(instanciador.DebugGol(1));

        }
        else if (other.gameObject.CompareTag("trave") && !jaColidiu)
        {
            StartCoroutine(ExplodirBola());
            instanciador.StartCoroutine(instanciador.DebugGol(2));

        }
        else if (other.gameObject.CompareTag("goleiro") && !jaColidiu)
        {
            StartCoroutine(ExplodirBola());
            instanciador.StartCoroutine(instanciador.DebugGol(3));
        }
    }

    public IEnumerator ExplodirBola()
    {
        jaColidiu = true;

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        GameObject prefab = Resources.Load<GameObject>("explosao");
        Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
    }

    public IEnumerator ExplodirBolaGol()
    {
        Destroy(gameObject);
        GameObject prefab = Resources.Load<GameObject>("explosao2");
        Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
    }
}
