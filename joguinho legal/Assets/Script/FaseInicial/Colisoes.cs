using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class Colisoes : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject txtportatrancada; // Texto exibido quando a porta está trancada
    public PlayableDirector cutscene; // Diretor de cutscene para a animação
    public Movimento2 script; // Referência ao script de movimento (não utilizado no código atual)

    void Start()
    {
        // Inicializa o script (nenhuma ação necessária no momento)
    }

    void OnCollisionEnter(Collision other)
    {
        // Troca de cena com base na colisão
        if (other.gameObject.CompareTag("predio"))
        {
            SceneManager.LoadScene("Predio"); // Carrega a cena "Predio"
        }
        else if (other.gameObject.CompareTag("taverna"))
        {
            SceneManager.LoadScene("Taverna"); // Carrega a cena "Taverna"
        }
        else if (other.gameObject.CompareTag("cassino"))
        {
            SceneManager.LoadScene("Cassino"); // Carrega a cena "Cassino"
        }
        else if (other.gameObject.CompareTag("voltar"))
        {
            SceneManager.LoadScene("CenaInicial"); // Volta para a cena inicial
        }
        else if (other.gameObject.CompareTag("portatrancada"))
        {
            txtportatrancada.SetActive(true); // Exibe o texto de porta trancada
            Invoke("DesativarTxt", 2); // Desativa o texto após 2 segundos
        }
        else if (other.gameObject.CompareTag("portafundo"))
        {
            MudarFase();
            //cutscene.Play(); // Inicia a cutscene
            //Invoke("MudarFase", (float)cutscene.duration); // Troca de cena após o fim da cutscene
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
}
