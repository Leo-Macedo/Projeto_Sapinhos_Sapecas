using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VerificarFasesTaverna : MonoBehaviour
{
    public int controladorFases = 0;

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
                Bar();
                break;

            case 1:
                Porao();
                break;

            case 2:
                SalaZida();
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
        }
    }

    public void Bar()
    {
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        cutsceneBar.Play();
    }

    public void Porao()
    {
        cutscenePorao.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());

        Invoke("DesativarOBJ1", (float)cutscenePorao.duration);
        player.transform.position = pontoNascer1.position;
    }

    public void SalaZida()
    {
        cutsceneSalaZida.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());

        Invoke("DesativarOBJ2", (float)cutsceneSalaZida.duration);

        player.transform.position = pontoNascer2.position;
    }

    private IEnumerator ControlarMovimentoDuranteCutscene()
    {
        // Zera a velocidade
        movimento2.veloAndando = 0f;
        movimento2.veloCorrendo = 0f;
        yield return new WaitForSeconds(0.5f);

        freeLookCamera.enabled = false;

        // Espera pela duração da cutscene
        yield return new WaitForSeconds((float)cutsceneBar.duration);

        // Restaura as velocidades iniciais
        movimento2.veloAndando = veloAndandoInicial;
        movimento2.veloCorrendo = veloCorrendoInicial;
        freeLookCamera.enabled = true;
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
