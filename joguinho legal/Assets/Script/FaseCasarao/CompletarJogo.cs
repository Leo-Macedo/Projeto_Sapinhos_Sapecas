using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CompletarJogo : MonoBehaviour
{
    public VidaVilao vidaVilao;
    private bool podeAbrir;
    private bool abrir;
    private bool abriu;
    public GameObject txtAbrir;
    public GameObject txtFAbrir;
    public Animator porta;
    public Animator fade;
    public PlayableDirector playableDirector;

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (vidaVilao.morreuvilao)
        {
            podeAbrir = true;
            txtAbrir.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F) && abrir && !abriu) 
        {
            abriu = true;
            StartCoroutine(ComeçarCutscene());
        }
    }

    public IEnumerator ComeçarCutscene()
    {
        txtFAbrir.SetActive(false);
        porta.SetTrigger("abrir");
        yield return new WaitForSeconds(2f);
        fade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        playableDirector.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cadeia") && podeAbrir)
        {
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
