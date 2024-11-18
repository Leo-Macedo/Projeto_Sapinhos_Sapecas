using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirComanda : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    public GameObject comanda;
    public GameObject txtF;
    private bool podeAbrir;

    void Start() { }

    void Update()
    {
        if (podeAbrir && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Abrircomanda());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            podeAbrir = true;
            txtF.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            podeAbrir = false;
            comanda.SetActive(false);
            txtF.SetActive(false);
        }
    }

    public IEnumerator Abrircomanda()
    {
        controleSensibilidadeCamera.podePausar = false;

        comanda.SetActive(true);
        yield return new WaitForSeconds(3f);
        comanda.SetActive(false);
        controleSensibilidadeCamera.podePausar = true;
    }
}
