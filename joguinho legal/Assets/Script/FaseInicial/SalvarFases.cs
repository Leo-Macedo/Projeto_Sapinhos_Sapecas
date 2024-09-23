using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class SalvarFases : MonoBehaviour
{
    // Constantes para as chaves do PlayerPrefs
    private const string PredioCompletadoKey = "PredioCompletado";
    private const string TavernaCompletadaKey = "TavernaCompletada";
    private const string CassinoCompletadoKey = "CassinoCompletado";

    [Header("Objetos")]
    public GameObject cassino;
    public GameObject taverna;
    public GameObject casarao;

    [Header("CutScenes")]
    public GameObject GOCutsceneInicial;
    public PlayableDirector cutsceneInicial;
    public PlayableDirector cutsceneTaverna;
    public PlayableDirector cutsceneCassino;
    public PlayableDirector cutsceneCasarao;

    [Header("Sapos")]
    public CinemachineFreeLook freeLookCamera;
    public GameObject ronaldinhoAnda;
    public GameObject ronaldinhoAtor;
    public GameObject rivaldinhoAnda;
    public GameObject rivaldinhoAtor;
    public GameObject romarinhoAnda;
    public GameObject romarinhoAtor;

    [Header("Martinhas")]
    public GameObject martinha1;
    public GameObject martinha2;
    public GameObject martinha3;
    public GameObject martinha4;

    [Header("Scripts")]
    private Movimento2 movimento2RN;
    private Movimento2 movimento2RV;
    private Movimento2 movimento2RM;

    [Header("Telas HUD")]
    public GameObject telaRonaldinho;
    public GameObject telaRivaldinho;
    public GameObject telaRomarinho;
    public GameObject telaTodos;

    // Start is called before the first frame update
    void Start()
    {
        VerificarProgresso();
    }

    // Verifica o progresso do jogador e desbloqueia as fases correspondentes
    public void VerificarProgresso()
    {
        // Resetar o estado dos objetos antes de verificar o progresso
        ResetarObjetos();

        if (PlayerPrefs.GetInt(PredioCompletadoKey, 0) == 0)
        {
            InicioJogo();
            return;
        }

        if (PlayerPrefs.GetInt(TavernaCompletadaKey, 0) == 0)
        {
            TavernaDesbloqueada();
            return;
        }

        if (PlayerPrefs.GetInt(CassinoCompletadoKey, 0) == 0)
        {
            CassinoDesbloqueado();
            return;
        }

        CasaraoDesbloqueado();
    }

    // Método para resetar o estado dos objetos
    private void ResetarObjetos()
    {
        // Desative todos os objetos que são ativados por progresso
        martinha1.SetActive(false);
        martinha2.SetActive(false);
        martinha3.SetActive(false);
        martinha4.SetActive(false);

        telaRonaldinho.SetActive(false);
        telaRivaldinho.SetActive(false);
        telaRomarinho.SetActive(false);
        telaTodos.SetActive(false);

        ronaldinhoAnda.SetActive(false);
        ronaldinhoAtor.SetActive(true);

        rivaldinhoAnda.SetActive(false);
        rivaldinhoAtor.SetActive(true);

        romarinhoAnda.SetActive(false);
        romarinhoAtor.SetActive(true);

        cassino.SetActive(false);
        taverna.SetActive(false);
        casarao.SetActive(false);
    }

    // Inicia o jogo com a cutscene
    public void InicioJogo()
    {
        Invoke("PodeAndar", (float)cutsceneInicial.duration);
        Debug.Log("Metodo chamado InicioJogo e não completou fases");
        martinha1.SetActive(true);
        GOCutsceneInicial.SetActive(true);
        cutsceneInicial.Play();
    }

    // Desbloqueia a Taverna
    public void TavernaDesbloqueada()
    {
        Debug.Log("Metodo chamado TavernaDesbloqueada e fase1 predio completada");
        cutsceneTaverna.Play();
        taverna.SetActive(true);
        martinha2.SetActive(true);
        telaRivaldinho.SetActive(true);
        rivaldinhoAnda.SetActive(true);
        rivaldinhoAtor.SetActive(false);
        GOCutsceneInicial.SetActive(false);
        freeLookCamera.Follow = rivaldinhoAnda.transform;
        freeLookCamera.LookAt = rivaldinhoAnda.transform;
    }

    // Desbloqueia o Cassino
    public void CassinoDesbloqueado()
    {
        Debug.Log("Metodo chamado CassinoDesbloqueado e fase2 taverna completada");

        cutsceneCassino.Play();
        cassino.SetActive(true);
        taverna.SetActive(true);
        martinha3.SetActive(true);
        telaRomarinho.SetActive(true);
        romarinhoAnda.SetActive(true);
        romarinhoAtor.SetActive(false);
        GOCutsceneInicial.SetActive(false);
        freeLookCamera.Follow = romarinhoAnda.transform;
        freeLookCamera.LookAt = romarinhoAnda.transform;
    }

    // Desbloqueia o Casarão
    public void CasaraoDesbloqueado()
    {
        Debug.Log("Metodo chamado CasaraoDesbloqueado e fase3 taverna completada");
        cutsceneCasarao.Play();
        casarao.SetActive(true);
        taverna.SetActive(true);
        cassino.SetActive(true);
        martinha4.SetActive(true);
        telaTodos.SetActive(true);
        ronaldinhoAnda.SetActive(true);
        ronaldinhoAtor.SetActive(false);        
        GOCutsceneInicial.SetActive(false);
        freeLookCamera.Follow = ronaldinhoAnda.transform;
        freeLookCamera.LookAt = ronaldinhoAnda.transform;
    }

    public void PodeAndar()
    {
        ronaldinhoAnda.SetActive(true);
        ronaldinhoAtor.SetActive(false);
        telaRonaldinho.SetActive(true);
    }
}
