using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VerificarFasesTaverna : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    public int controladorFases = 0;
    public EscudoFuncionando escudoFuncionando;

    [Header("Hud")]
    public GameObject vidaMelo;
    public GameObject vidaPlayer;
    public GameObject super;
    public GameObject escudos;
    public TextMeshProUGUI txtEscudosPegos;

    [Header("Referências")]
    public GameObject gameOverCanvas;
    public GameObject player;
    public Transform pontoNascer1;
    public Transform pontoNascer2;
    public GameObject cilindroVilao;
    public GameObject cilindroCapanga;

    [Header("CutScenes")]
    public PlayableDirector cutsceneBar;
    public PlayableDirector cutscenePorao;
    public PlayableDirector cutsceneSalaZida;

    [Header("Câmera")]
    public CinemachineFreeLook freeLookCamera;

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject[] mensagem;
    public Animator[] animatorMSG;

    private VidaPersonagem vidaPersonagem;
    private Movimento2 movimento2;
    private float veloAndandoInicial;
    private float veloCorrendoInicial;

    void Start()
    {
        Time.timeScale = 1f;

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        movimento2 = player.GetComponent<Movimento2>();
        controladorFases = PlayerPrefs.GetInt("ControladorFasesTaverna", 0);

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
        controladorFases = PlayerPrefs.GetInt("ControladorFasesTaverna", 0);
    }

    void CarregarRound()
    {
        // Define a posição do player com base no controlador de fases
        switch (controladorFases)
        {
            case 0:
                StartCoroutine(Bar());
                break;

            case 1:
                StartCoroutine(Porao());
                break;

            case 2:
                StartCoroutine(SalaZida());
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
            Time.timeScale = 0f; // Pausa o jogo
            gameOverCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public IEnumerator Bar()
    {
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneBar.Play();
        yield return new WaitForSeconds((float)cutsceneBar.duration);
        controleSensibilidadeCamera.podePausar = true;

        somNoti.Play();
        mensagem[0].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[0].SetTrigger("fechou");
        somNoti.Play();
        mensagem[1].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[1].SetTrigger("fechou");
        somNoti.Play();
        mensagem[2].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[2].SetTrigger("fechou");
    }

    public IEnumerator Porao()
    {
        super.SetActive(true);
        escudos.SetActive(true);
        cutscenePorao.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());

        Invoke("DesativarOBJ1", (float)cutscenePorao.duration);
        player.transform.position = pontoNascer1.position;

        yield return new WaitForSeconds((float)cutscenePorao.duration);
        controleSensibilidadeCamera.podePausar = true;

        somNoti.Play();
        mensagem[3].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[3].SetTrigger("fechou");
        somNoti.Play();
        mensagem[4].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[4].SetTrigger("fechou");
        somNoti.Play();
        mensagem[5].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[5].SetTrigger("fechou");
    }

    public IEnumerator SalaZida()
    {
        super.SetActive(true);
        vidaMelo.SetActive(true);
        vidaPlayer.SetActive(true);
        escudos.SetActive(true);
        escudoFuncionando.contadorEscudo = 3;
        txtEscudosPegos.text = escudoFuncionando.contadorEscudo + "";
        cutsceneSalaZida.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());

        Invoke("DesativarOBJ2", (float)cutsceneSalaZida.duration);

        player.transform.position = pontoNascer2.position;

        yield return new WaitForSeconds((float)cutsceneSalaZida.duration);
        controleSensibilidadeCamera.podePausar = true;

        somNoti.Play();
        mensagem[6].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[6].SetTrigger("fechou");
        somNoti.Play();
        mensagem[7].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[7].SetTrigger("fechou");
        somNoti.Play();
        mensagem[8].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[8].SetTrigger("fechou");
    }

    private IEnumerator ControlarMovimentoDuranteCutscene()
    {
        // Zera a velocidade
        movimento2.veloAndando = 0f;
        movimento2.veloCorrendo = 0f;
        yield return new WaitForSeconds(0.5f);


        // Espera pela duração da cutscene
        yield return new WaitForSeconds((float)cutsceneBar.duration);

        // Restaura as velocidades iniciais
        movimento2.veloAndando = veloAndandoInicial;
        movimento2.veloCorrendo = veloCorrendoInicial;
    }

    private void DesativarOBJ1()
    {
        cilindroCapanga.SetActive(false);
    }

    private void DesativarOBJ2()
    {
        cilindroVilao.SetActive(false);
    }

    public void AtualizarControladorFases()
    {
        controladorFases += 1;
        PlayerPrefs.SetInt("ControladorFasesTaverna", controladorFases);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = " + controladorFases);
    }
}
