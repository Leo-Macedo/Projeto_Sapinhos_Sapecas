using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement; // Importante para reiniciar a cena

public class VerificarFaseCassino : MonoBehaviour
{
    private int controladorFases = 0;
    public CinemachineFreeLook freeLookCamera;
    public GameObject portalvoltar;
    public GameObject txtvaaoportal;
    public GameObject gameOverCanvas; // Referência ao Canvas de Game Over

    [Header("Round1")]
    public GameObject romarinho1;
    private VidaPersonagem vidaPersonagem1;
    public GameObject round1;
    public GameObject melo1;
    private VidaVilao vidaVilao1;
    private bool round1Acabou;
    public PlayableDirector cutSceneRound1;
    public PlayableDirector cutSceneRound1Acabou;

    [Header("Round2")]
    public GameObject romarinho2;
    private VidaPersonagem vidaPersonagem2;
    public GameObject round2;
    public GameObject melo2;
    private VidaVilao vidaVilao2;
    private bool round2Acabou;
    public PlayableDirector cutSceneRound2;

    [Header("Round3")]
    public GameObject romarinho3;
    private VidaPersonagem vidaPersonagem3;
    public GameObject round3;
    public GameObject melo3;
    private VidaVilao vidaVilao3;
    private bool round3Acabou;
    public PlayableDirector cutSceneRound3;

    void Start()
    {
        vidaVilao1 = melo1.GetComponent<VidaVilao>();
        vidaVilao2 = melo2.GetComponent<VidaVilao>();
        vidaVilao3 = melo3.GetComponent<VidaVilao>();

        vidaPersonagem1 = romarinho1.GetComponent<VidaPersonagem>();
        vidaPersonagem2 = romarinho2.GetComponent<VidaPersonagem>();
        vidaPersonagem3 = romarinho3.GetComponent<VidaPersonagem>();

        VerificarProgresso();
    }

    void Update()
    {
        VerificarRodadas();
        VerificarMorteJogador(); // Verifica se o jogador morreu
    }

    public void VerificarProgresso()
    {
        if (controladorFases == 0)
        {
            Round1();
        }
        else if (controladorFases == 1)
        {
            Round2();
        }
        else if (controladorFases == 2)
        {
            Round3();
        }
        else if (controladorFases == 3)
        {
            Vitoria();
        }
    }

    private void VerificarRodadas()
    {
        if (vidaVilao1 != null && vidaVilao1.Vida <= 0 && !round1Acabou)
        {
            round1Acabou = true;
            controladorFases += 1;
            VerificarProgresso();
        }
        if (vidaVilao2 != null && vidaVilao2.Vida <= 0 && !round2Acabou)
        {
            round2Acabou = true;
            controladorFases += 1;
            VerificarProgresso();
        }
        if (vidaVilao3 != null && vidaVilao3.Vida <= 0 && !round3Acabou)
        {
            round3Acabou = true;
            controladorFases += 1;
            VerificarProgresso();
        }
    }

    private void VerificarMorteJogador()
    {
        switch (controladorFases)
        {
            case 0:
                if (vidaPersonagem1 != null && vidaPersonagem1.vidaAtual <= 0)
                {
                    GameOver();
                }
                break;
            case 1:
                if (vidaPersonagem2 != null && vidaPersonagem2.vidaAtual <= 0)
                {
                    GameOver();
                }
                break;
            case 2:
                if (vidaPersonagem3 != null && vidaPersonagem3.vidaAtual <= 0)
                {
                    GameOver();
                }
                break;
        }
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true); // Ativa a tela de Game Over
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void ReiniciarRound()
    {
        Time.timeScale = 1f; // Despausa o jogo
        switch (controladorFases)
        {
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia no Round 1
                gameOverCanvas.SetActive(false);
                Round1();
                break;
            case 1:
                AtivarRound2();
                gameOverCanvas.SetActive(false);

                vidaVilao2.Vida = vidaVilao2.VidaInicial;
                vidaPersonagem2.vidaAtual = vidaPersonagem2.vidaInicial;
                break;
            case 2:
                AtivarRound3();
                gameOverCanvas.SetActive(false);

                vidaVilao3.Vida = vidaVilao3.VidaInicial;
                vidaPersonagem3.vidaAtual = vidaPersonagem3.vidaInicial;
                break;
        }
    }

    public void Round1()
    {
        round1.SetActive(true);
        cutSceneRound1.Play();
    }

    public void Round2()
    {
        cutSceneRound1Acabou.Play();
        Invoke("AtivarRound2", (float)cutSceneRound1Acabou.duration);
    }

    public void Round3()
    {
        cutSceneRound1Acabou.Play();
        Invoke("AtivarRound3", (float)cutSceneRound1Acabou.duration);
    }

    public void AtivarRound2()
    {
        DestruirTodosCapangas();
        cutSceneRound2.Play();
        round2.SetActive(true);
        round1.SetActive(false);
        freeLookCamera.Follow = romarinho2.transform;
        freeLookCamera.LookAt = romarinho2.transform;
    }

    public void AtivarRound3()
    {
        DestruirTodosCapangas();
        cutSceneRound3.Play();
        round3.SetActive(true);
        round2.SetActive(false);
        freeLookCamera.Follow = romarinho3.transform;
        freeLookCamera.LookAt = romarinho3.transform;
    }

    public void DestruirTodosCapangas()
    {
        GameObject[] capangas = GameObject.FindGameObjectsWithTag("capanga");
        foreach (GameObject capanga in capangas)
        {
            Destroy(capanga);
        }
        Debug.Log("Todos os capangas foram destruídos.");
    }

    public void Vitoria()
    {
        cutSceneRound1Acabou.Play();
        portalvoltar.SetActive(true);
        txtvaaoportal.SetActive(true);
        PlayerPrefs.SetInt("CassinoCompletado", 1);
    }
}
