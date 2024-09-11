using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VerificarFasePredio : MonoBehaviour
{
    public int controladorFases = 0;

    [Header("Referências")]
    public GameObject gameOverCanvas;
    public GameObject player;
    public Transform pontoNascer1;
    public Transform pontoNascer2;
    public Transform pontoNascer3;

    private VidaPersonagem vidaPersonagem;

    void Start()
    {
        Time.timeScale = 1f;
        
        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        controladorFases = PlayerPrefs.GetInt("ControladorFases", 0);
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
        controladorFases = PlayerPrefs.GetInt("ControladorFases", 0);
    }

    void CarregarRound()
    {
        // Define a posição do player com base no controlador de fases
        switch (controladorFases)
        {
            case 0:
                Andar1();
                break;

            case 1:
                Andar2();
                break;

            case 2:
                Andar3();
                break;

            case 3:
                Andar4();
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

    public void Andar1() 
    {
        // Pode definir uma posição padrão ou uma lógica específica para o andar 1
    }

    public void Andar2()
    {
        player.transform.position = pontoNascer1.position;
    }

    public void Andar3()
    {
        player.transform.position = pontoNascer2.position;
    }

    public void Andar4()
    {
        player.transform.position = pontoNascer3.position;
    }

    public void AtualizarControladorFases()
    {
        controladorFases += 1;
        PlayerPrefs.SetInt("ControladorFases", controladorFases);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = " + controladorFases);
    }
}
