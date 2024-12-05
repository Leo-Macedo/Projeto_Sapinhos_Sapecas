using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CompletarJogo : MonoBehaviour
{
    public CompletarJogo2 completarJogo2;
    public VidaVilao vidaVilao;
    private bool podeAbrir;
    private bool abrir;
    private bool abriu;
    public GameObject txtAbrir;
    public GameObject txtFAbrir;
    
   
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (vidaVilao.morreuvilao)
        {
            podeAbrir = true;
            txtAbrir.SetActive(true);
        }

        if (Input.GetButtonDown("Interagir") && abrir && !abriu)
        {
            txtFAbrir.SetActive(false);
            abriu = true;
            completarJogo2.StartCoroutine(completarJogo2.ComeçarCutscene());
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cadeia") && podeAbrir)
        {
            txtAbrir.SetActive(false);
            txtFAbrir.SetActive(true);
            abrir = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("cadeia") && podeAbrir)
        {
            txtFAbrir.SetActive(false);
            abrir = false;
        }
    }
}
