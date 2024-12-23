using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VerificarFasePredio : MonoBehaviour
{
    public GameObject capangas2;
    public GameObject capangas3;
    public NavMeshAgent melo;
    private NavMeshAgent[] allAgents;
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    public int controladorFases = 0;
    public PassarFases passarFases;

    [Header("Hud")]
    public GameObject vidaMelo;
    public GameObject vidaPlayer;
    public GameObject Super;

    [Header("Referências")]
    public GameObject gameOverCanvas;
    public GameObject player;
    public Transform pontoNascer1;
    public Transform pontoNascer2;
    public Transform pontoNascer3;


    [Header("CutScenes")]
    public PlayableDirector cutsceneAndar1;
    public PlayableDirector cutsceneAndar2;
    public PlayableDirector cutsceneAndar3;
    public PlayableDirector cutsceneAndar4;

    [Header("Câmera")]
    public CinemachineFreeLook freeLookCamera;

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject mensagem1;
    public GameObject mensagem2;
    public GameObject mensagem3;
    public GameObject mensagem4;
    public GameObject mensagem5;
    public GameObject mensagem6;

    public GameObject mensagem11;
    public GameObject mensagem12;
    public GameObject mensagem13;

    private Animator animMensagem1;
    private Animator animMensagem2;
    private Animator animMensagem3;
    private Animator animMensagem4;
    private Animator animMensagem5;
    private Animator animMensagem6;
    private Animator animMensagem11;
    private Animator animMensagem12;
    private Animator animMensagem13;

    private VidaPersonagem vidaPersonagem;
    private Movimento2 movimento2;
    private float veloAndandoInicial;
    private float veloCorrendoInicial;
    public SuperRonaldinho superRonaldinho;

    void Start()
    {
        allAgents = Object.FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        

        animMensagem1 = mensagem1.GetComponent<Animator>();
        animMensagem2 = mensagem2.GetComponent<Animator>();
        animMensagem3 = mensagem3.GetComponent<Animator>();
        animMensagem4 = mensagem4.GetComponent<Animator>();
        animMensagem5 = mensagem5.GetComponent<Animator>();
        animMensagem6 = mensagem6.GetComponent<Animator>();

        animMensagem11 = mensagem11.GetComponent<Animator>();
        animMensagem12 = mensagem12.GetComponent<Animator>();
        animMensagem13 = mensagem13.GetComponent<Animator>();

        Time.timeScale = 1f;

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        movimento2 = player.GetComponent<Movimento2>();
        controladorFases = PlayerPrefs.GetInt("ControladorFasesPredio", 0);

        veloAndandoInicial = movimento2.veloAndando;
        veloCorrendoInicial = movimento2.veloCorrendo;

        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no meio da tela
        Cursor.visible = false; // Torna o cursor invisível
        CarregarRound(); // Define a posição inicial do jogador ao iniciar
    }

    void Update()
    {
        VerificarMorteJogador();
    }

    public void ReiniciarRound()
    {
        Time.timeScale = 1f; // Despausa o jogo

        // Desativa o canvas de Game Over
        gameOverCanvas.SetActive(false);

        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Atualiza o controlador de fases após recarregar a cena
        controladorFases = PlayerPrefs.GetInt("ControladorFasesPredio", 0);
    }

    void CarregarRound()
    {
        // Define a posição do player com base no controlador de fases
        switch (controladorFases)
        {
            case 0:
                StartCoroutine(Andar1());
                break;

            case 1:
                StartCoroutine(Andar2());

                break;

            case 2:
                StartCoroutine(Andar3());

                break;

            case 3:
                StartCoroutine(Andar4());
                break;

            default:
                Debug.LogWarning("Controlador de fases desconhecido: " + controladorFases);
                break;
        }
    }

    private void VerificarMorteJogador()
    {
        if (vidaPersonagem.vidaAtual <= 0)
        {
            // Pausa o jogo
            Time.timeScale = 0f;
            gameOverCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Encontra todos os objetos com a tag "musica" e para o AudioSource deles
            GameObject[] objetosMusica = GameObject.FindGameObjectsWithTag("musica");
            foreach (GameObject objeto in objetosMusica)
            {
                AudioSource audioSource = objeto.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    public IEnumerator Andar1()
    {
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneAndar1.Play();
        vidaPersonagem.vidaAtual = 3;
        superRonaldinho.podeUtar = false;
        yield return new WaitForSeconds((float)cutsceneAndar1.duration);

        controleSensibilidadeCamera.podePausar = true;
        somNoti.Play();
        mensagem1.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem1.SetTrigger("fechou");
        somNoti.Play();
        mensagem2.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem2.SetTrigger("fechou");
    }

    public IEnumerator Andar2()
    {
        vidaPersonagem.vidaAtual = 3;
        superRonaldinho.podeUtar = true;

        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneAndar2.Play();
        player.transform.position = pontoNascer1.position;
        passarFases.boolandar2 = true;
        vidaPlayer.SetActive(true);
        Super.SetActive(true);

        yield return new WaitForSeconds((float)cutsceneAndar2.duration);
        foreach (NavMeshAgent agent in allAgents)
        {
            agent.enabled = true;
        }
        controleSensibilidadeCamera.podePausar = true;
        capangas2.SetActive(true);
        somNoti.Play();
        mensagem3.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem3.SetTrigger("fechou");
        somNoti.Play();
        mensagem4.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem4.SetTrigger("fechou");
        somNoti.Play();
        mensagem5.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem5.SetTrigger("fechou");
    }

    public IEnumerator Andar3()
    {
        vidaPersonagem.vidaAtual = 3;
        superRonaldinho.podeUtar = true;
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneAndar3.Play();
        passarFases.AtivarCapangas("CapangaAndar3", 3);
        passarFases.boolandar3 = true;
        passarFases.boolandar2 = false;
        player.transform.position = pontoNascer2.position;
        vidaPlayer.SetActive(true);
        Super.SetActive(true);

        yield return new WaitForSeconds((float)cutsceneAndar3.duration);
        foreach (NavMeshAgent agent in allAgents)
        {
            if (agent != null && agent.gameObject != null) // Verifica se o agente ainda existe
            {
                agent.enabled = true;
            }
        }
        capangas3.SetActive(true);
        controleSensibilidadeCamera.podePausar = true;

        somNoti.Play();
        mensagem6.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem6.SetTrigger("fechou");
    }

    public IEnumerator Andar4()
    {
        
        vidaPersonagem.vidaAtual = 3;
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneAndar4.Play();
        player.transform.position = pontoNascer3.position;
        vidaPlayer.SetActive(true);
        Super.SetActive(true);
        vidaMelo.SetActive(true);
        superRonaldinho.podeUtar = true;

        yield return new WaitForSeconds((float)cutsceneAndar3.duration);
        melo.enabled = true;
        controleSensibilidadeCamera.podePausar = true;

        somNoti.Play();
        mensagem11.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem11.SetTrigger("fechou");
        somNoti.Play();
        mensagem12.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem12.SetTrigger("fechou");
        somNoti.Play();
        mensagem13.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem13.SetTrigger("fechou");
    }

    private IEnumerator ControlarMovimentoDuranteCutscene()
    {
        // Zera a velocidade
        movimento2.veloAndando = 0f;
        movimento2.veloCorrendo = 0f;
        yield return new WaitForSeconds(0.5f);

        freeLookCamera.enabled = false;

        // Espera pela duração da cutscene
        yield return new WaitForSeconds((float)cutsceneAndar1.duration);

        // Restaura as velocidades iniciais
        movimento2.veloAndando = veloAndandoInicial;
        movimento2.veloCorrendo = veloCorrendoInicial;
        freeLookCamera.enabled = true;
    }

    public void AtualizarControladorFases()
    {
        controladorFases += 1;
        PlayerPrefs.SetInt("ControladorFasesPredio", controladorFases);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = " + controladorFases);
    }
}
