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
        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        controladorFases = PlayerPrefs.GetInt("ControladorFases", 0);
    }
    public void ReiniciarRound()
    {
        Time.timeScale = 1f; // Despausa o jogo
        
        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 

        // Desativa o canvas de Game Over
        gameOverCanvas.SetActive(false);

        // Define a posição do player com base no controlador de fases
        switch (controladorFases)
        {
            case 0:
                player.transform.position = pontoNascer1.position;
                break;
            case 1:
                player.transform.position = pontoNascer2.position;
                break;
            case 2:
                player.transform.position = pontoNascer3.position;
                break;
            default:
                Debug.LogWarning("Controlador de fases desconhecido: " + controladorFases);
                break;
        }
    }
    private void VerificarMorteJogador()
    {
        if(vidaPersonagem.vidaAtual <= 0)
        {
            Time.timeScale = 0f;
            gameOverCanvas.SetActive(true);
        }
    }

    public void AtualizarControladorFases()
    {
        controladorFases += 1;
        PlayerPrefs.SetInt("ControladorFases", controladorFases);
    }
}
