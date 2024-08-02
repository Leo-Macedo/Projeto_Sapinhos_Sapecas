using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class Colisoes : MonoBehaviour
{
    //Texto porta trancada
    public GameObject txtportatrancada;
    public PlayableDirector cutscene;
    public GameObject luz;

    //Camera da CutScene
    public Movimento2 script;

    //Só poder mover após acabar CutScene
    void Start()
    {
        AcenderLuz();
    }

    //Colisões para trocar de cena
    public void OnCollisionEnter(Collision other)
    {
        
        //Ir para fase prédio
        if (other.gameObject.CompareTag("predio"))
            SceneManager.LoadScene("Predio");

        //Ir para fase taverna
        if (other.gameObject.CompareTag("taverna"))
            SceneManager.LoadScene("Taverna");

        //Ir para fase cassino
        if (other.gameObject.CompareTag("cassino"))
            SceneManager.LoadScene("Cassino");

        //Voltar para cena inicial
        if (other.gameObject.CompareTag("voltar"))
            SceneManager.LoadScene("CenaInicial");

        //Voltar para cena inicial
        if (other.gameObject.CompareTag("portatrancada"))
        {
            txtportatrancada.SetActive(true);
        Invoke("DesativarTxt", 2);
        }

         //Entrar casarao
        if (other.gameObject.CompareTag("portafundo"))
        {
            cutscene.Play();
        Invoke("MudarFase", (float)cutscene.duration);
        }


    }
    

    void DesativarTxt()
    {
        txtportatrancada.SetActive(false);
    }

    void MudarFase()
    {
        SceneManager.LoadScene("Casarao");

    }

    void AcenderLuz()
    {
        luz.SetActive(true);
        Invoke("ApagarLuz", 0.1f);
    }

      void ApagarLuz()
    {
        luz.SetActive(false);
        Invoke("AcenderLuz", 0.1f);
    }
}
