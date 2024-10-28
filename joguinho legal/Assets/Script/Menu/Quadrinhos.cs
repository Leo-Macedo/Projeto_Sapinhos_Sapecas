using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quadrinhos : MonoBehaviour
{
    public List<CanvasGroup> quadrinhosCanvasGroups; // Lista de CanvasGroups dos quadrinhos
    public float fadeDuration = 1.0f; // Duração do fade
    private int indiceAtual = 0; // Índice do quadrinho atual
    public Animator animatorFade;
    public GameObject telas;
    public GameObject quadrinhos;

    private void Start()
    {
        // Configura todos os quadrinhos como invisíveis, exceto o primeiro
        for (int i = 0; i < quadrinhosCanvasGroups.Count; i++)
        {
            quadrinhosCanvasGroups[i].alpha = (i == 0) ? 1.0f : 0.0f;
            quadrinhosCanvasGroups[i].interactable = (i == 0);
            quadrinhosCanvasGroups[i].blocksRaycasts = (i == 0);
        }
    }

    // Método para iniciar a história em quadrinhos
    public void IniciarQuadrinhos()
    {
        indiceAtual = 0;
        StartCoroutine(IniciarSequenciaQuadrinhos());
    }

    private IEnumerator IniciarSequenciaQuadrinhos()
    {
        // Ativa o trigger "fechar" e espera 2 segundos antes de iniciar o primeiro quadrinho
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        telas.SetActive(false);
        quadrinhos.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        animatorFade.SetTrigger("abrir");

        // Começa a exibição do primeiro quadrinho
        yield return StartCoroutine(FadeIn(quadrinhosCanvasGroups[indiceAtual]));
    }

    private void Update()
    {
        // Avança para o próximo quadrinho ao pressionar a tecla F
        if (Input.GetKeyDown(KeyCode.F) && indiceAtual < quadrinhosCanvasGroups.Count - 1)
        {
            StartCoroutine(
                FadeOutIn(
                    quadrinhosCanvasGroups[indiceAtual],
                    quadrinhosCanvasGroups[indiceAtual + 1]
                )
            );
            indiceAtual++;
        }
        // Ao chegar no último, troca para a próxima cena
        else if (Input.GetKeyDown(KeyCode.F) && indiceAtual == quadrinhosCanvasGroups.Count - 1)
        {
            StartCoroutine(TrocarCena());
        }
    }

    private IEnumerator FadeOutIn(CanvasGroup telaFechar, CanvasGroup telaAbrir)
    {
        yield return StartCoroutine(FadeOut(telaFechar));
        yield return StartCoroutine(FadeIn(telaAbrir));
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public IEnumerator TrocarCena()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs resetado no Build.");
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("CenaInicial");
    }
}
