using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SalvarFases : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    // Constantes para as chaves do PlayerPrefs
    private const string PredioCompletadoKey = "PredioCompletado";
    private const string TavernaCompletadaKey = "TavernaCompletada";
    private const string CassinoCompletadoKey = "CassinoCompletado";
    private const string TutorialCompletadoKey = "TutorialCompletado";

    [Header("Tutorial")]
    public GameObject paineltutorial;
    public Animator fade;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        VerificarProgresso();
    }

    // Verifica o progresso do jogador e desbloqueia as fases correspondentes
    public void VerificarProgresso()
    {
        // Resetar o estado dos objetos antes de verificar o progresso
        ResetarObjetos();

        if (
            PlayerPrefs.GetInt(PredioCompletadoKey, 0) == 0
            && PlayerPrefs.GetInt(TutorialCompletadoKey, 0) == 0
        )
        {
            InicioJogo();
            return;
        }
        if (PlayerPrefs.GetInt(TutorialCompletadoKey, 0) == 1)
        {
            Começar();
            return;
        }

        if (PlayerPrefs.GetInt(TavernaCompletadaKey, 0) == 0)
        {
            StartCoroutine(TavernaDesbloqueada());
            return;
        }

        if (PlayerPrefs.GetInt(CassinoCompletadoKey, 0) == 0)
        {
            StartCoroutine(CassinoDesbloqueado());
            return;
        }

        StartCoroutine(CasaraoDesbloqueado());
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
        Invoke("Tutorial", (float)cutsceneInicial.duration);
        Debug.Log("Metodo chamado InicioJogo e não completou fases");
        martinha1.SetActive(true);
        GOCutsceneInicial.SetActive(true);
        cutsceneInicial.Play();
    }

    // Desbloqueia a Taverna
    public IEnumerator TavernaDesbloqueada()
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

        yield return new WaitForSeconds((float)cutsceneTaverna.duration);
        controleSensibilidadeCamera.podePausar = true;
    }

    // Desbloqueia o Cassino
    public IEnumerator CassinoDesbloqueado()
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

        yield return new WaitForSeconds((float)cutsceneCassino.duration);
        controleSensibilidadeCamera.podePausar = true;
    }

    // Desbloqueia o Casarão
    public IEnumerator CasaraoDesbloqueado()
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

        yield return new WaitForSeconds((float)cutsceneCasarao.duration);
        controleSensibilidadeCamera.podePausar = true;
    }

    public void PodeAndar()
    {
        ronaldinhoAnda.SetActive(true);
        ronaldinhoAtor.SetActive(false);
        telaRonaldinho.SetActive(true);
    }

    public void Começar()
    {
        martinha1.SetActive(true);
        controleSensibilidadeCamera.podePausar = true;
        PodeAndar();
        PlayerPrefs.SetInt("TutorialCompletado", 2);
    }

    public void Tutorial()
    {
        paineltutorial.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ChamarCorrotinaTutorial()
    {
        StartCoroutine(MudarFase());
    }

    public IEnumerator MudarFase()
    {
        Time.timeScale = 1;
        fade.SetTrigger("fechar");
        paineltutorial.SetActive(false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Tutorial"); // Carrega a cena "Casarao"
    }
}
