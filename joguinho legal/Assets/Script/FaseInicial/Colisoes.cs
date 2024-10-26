using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Colisoes : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject txtportatrancada; // Texto exibido quando a porta está trancada
    public PlayableDirector cutscene; // Diretor de cutscene para a animação
    private Movimento2 movimento2;
    public Animator animatorFade;

    void Start()
    {
        movimento2 = GetComponent<Movimento2>();
    }

    void OnCollisionEnter(Collision other)
    {
        // Troca de cena com base na colisão
        if (other.gameObject.CompareTag("predio") && PlayerPrefs.GetInt("PredioCompletado", 0) == 0)
        {
            StartCoroutine(TrocarCena("Predio"));
        }
        else if (other.gameObject.CompareTag("taverna") && PlayerPrefs.GetInt("TavernaCompletada", 0) == 0)
        {
            StartCoroutine(TrocarCena("Taverna"));
        }
        else if (other.gameObject.CompareTag("cassino") && PlayerPrefs.GetInt("CassinoCompletado", 0) == 0)
        {
            StartCoroutine(TrocarCena("Cassino1"));
        }
        else if (other.gameObject.CompareTag("portatrancada"))
        {
            txtportatrancada.SetActive(true); // Exibe o texto de porta trancada
            Invoke("DesativarTxt", 3); // Desativa o texto após 2 segundos
        }
        else if (other.gameObject.CompareTag("portafundo"))
        {
            cutscene.Play(); // Inicia a cutscene
            Invoke("MudarFase", (float)cutscene.duration); // Troca de cena após o fim da cutscene
        }
    }

    void DesativarTxt()
    {
        txtportatrancada.SetActive(false); // Desativa o texto de porta trancada
    }

    void MudarFase()
    {
        SceneManager.LoadScene("Casarao"); // Carrega a cena "Casarao"
    }

    public IEnumerator TrocarCena(string nomeCena)
    {
        movimento2.veloAndando = 0;
        movimento2.veloCorrendo = 0;
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nomeCena);
    }
}
