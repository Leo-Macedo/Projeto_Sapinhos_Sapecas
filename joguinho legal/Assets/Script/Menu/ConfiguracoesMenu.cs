using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public AudioSource somclick;
    public Animator animatorFade;
    public CanvasGroup telaInicialCanvasGroup;
    public CanvasGroup telaPrincipalCanvasGroup;
    public CanvasGroup telaConfiguracoesCanvasGroup;
    public CanvasGroup telaCreditosCanvasGroup;
    public float fadeDuration = 1.0f; // Duração da transição (em segundos)

    // Variável para armazenar a tela atualmente ativa
    private CanvasGroup telaAtual;

    private void Start()
    
    {
        
        // Define a tela inicial e as opacidades
        telaAtual = telaInicialCanvasGroup;
        //telaInicialCanvasGroup.alpha = 1.0f; // Totalmente visível
        telaPrincipalCanvasGroup.alpha = 0.0f; // Invisível
        telaConfiguracoesCanvasGroup.alpha = 0.0f; // Invisível
        telaCreditosCanvasGroup.alpha = 0.0f; // Invisível
    }

    // Método para trocar de tela com fade
    public void TrocarTela(CanvasGroup novaTela)
    {
        if (telaAtual != novaTela)
        {
            // Inicia o fade out da tela atual e só depois faz o fade in da nova
            StartCoroutine(FadeOutIn(telaAtual, novaTela));
        }
    }

    private IEnumerator FadeOutIn(CanvasGroup telaFechar, CanvasGroup telaAbrir)
    {
        // Primeiro, faz o fade out da tela atual
        yield return StartCoroutine(FadeOut(telaFechar));

        // Depois de terminar o fade out, desativa interações com a tela anterior
        telaFechar.interactable = false;

        // Agora, faz o fade in da nova tela
        yield return StartCoroutine(FadeIn(telaAbrir));

        // Define a nova tela como a tela atual
        telaAtual = telaAbrir;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0; // Garante que seja 0 no final
        canvasGroup.interactable = false; // Desabilita interações com o painel
        canvasGroup.blocksRaycasts = false; // Evita cliques enquanto a tela está invisível
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        canvasGroup.interactable = true; // Habilita interações com o painel
        canvasGroup.blocksRaycasts = true; // Permite cliques na nova tela
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1; // Garante que seja 1 no final
    }

    // Funções chamadas pelos botões
    public void AbrirConfiguracoes()
    {
        TrocarTela(telaConfiguracoesCanvasGroup);
        somclick.Play();
    }

    public void AbrirCreditos()
    {
        TrocarTela(telaCreditosCanvasGroup);
                somclick.Play();

    }

    public void VoltarParaMenuPrincipal()
    {
        TrocarTela(telaPrincipalCanvasGroup);
                somclick.Play();

    }

    // Novo método para o botão Jogar
    public void Jogar()
    {
        TrocarTela(telaPrincipalCanvasGroup);
                somclick.Play();

    }

    public void Cassino()
    {
        SceneManager.LoadScene("Predio");
         PlayerPrefs.SetInt("JaTocou", 1);
        PlayerPrefs.GetInt("ControladorFasesCasarao", 2);
    }
    
}
