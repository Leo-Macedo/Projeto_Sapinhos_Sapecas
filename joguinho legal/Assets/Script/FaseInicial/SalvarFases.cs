using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SalvarFases : MonoBehaviour
{
    // Constantes para as chaves do PlayerPrefs
    private const string PredioCompletadoKey = "PredioCompletado";
    private const string TavernaCompletadaKey = "TavernaCompletada";
    private const string CassinoCompletadoKey = "CassinoCompletado";

    public PlayableDirector cutscene;
    public GameObject cassino;
    public GameObject taverna;
    public GameObject casarao;

    // Start is called before the first frame update
    void Start()
    {
        VerificarProgresso();
    }

    // Verifica o progresso do jogador e desbloqueia as fases correspondentes
    public void VerificarProgresso()
    {
        if (PlayerPrefs.GetInt(PredioCompletadoKey, 0) == 0)
        {
            InicioJogo();
            return; // Encerra a verificação se a primeira condição for verdadeira
        }

        if (PlayerPrefs.GetInt(TavernaCompletadaKey, 0) == 0)
        {
            TavernaDesbloqueada();
            return; // Encerra a verificação se a segunda condição for verdadeira
        }

        if (PlayerPrefs.GetInt(CassinoCompletadoKey, 0) == 0)
        {
            CassinoDesbloqueado();
            return; // Encerra a verificação se a terceira condição for verdadeira
        }

        // Se todas as fases anteriores foram completadas, desbloqueia o Casarão
        CasaraoDesbloqueado();
    }

    // Inicia o jogo com a cutscene
    public void InicioJogo()
    {
        Debug.Log("Metodo chamado InicioJogo e não completou fases");

        if (cutscene != null)
        {
            cutscene.Play();
        }
    }

    // Desbloqueia a Taverna
    public void TavernaDesbloqueada()
    {
        Debug.Log("Metodo chamado TavernaDesbloqueada e fase1 predio completada");

        if (taverna != null)
        {
            taverna.SetActive(true);
        }
    }

    // Desbloqueia o Cassino
    public void CassinoDesbloqueado()
    {
        Debug.Log("Metodo chamado CassinoDesbloqueado e fase2 taverna completada");

        if (cassino != null)
        {
            cassino.SetActive(true);
        }
    }

    // Desbloqueia o Casarão
    public void CasaraoDesbloqueado()
    {
        Debug.Log("Metodo chamado CasaraoDesbloqueado e fase3 taverna completada");

        if (casarao != null)
        {
            casarao.SetActive(true);
        }
    }
}
