using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuadrinhosFinais : MonoBehaviour
{
    public GameObject animator;
    public GameObject sair;
    public List<CanvasGroup> quadrinhosCanvasGroups; // Lista de CanvasGroups dos quadrinhos
    public float fadeDuration = 1.0f; // Duração do fade
    private int indiceAtual = 0; // Índice do quadrinho atual
    public Animator animatorFade;
    public GameObject quadrinhos;
    public Animator creditos;

    private void Start()
    {
        // Configura todos os quadrinhos como invisíveis, exceto o primeiro
        for (int i = 0; i < quadrinhosCanvasGroups.Count; i++)
        {
            quadrinhosCanvasGroups[i].alpha = (i == 0) ? 1.0f : 0.0f;
            quadrinhosCanvasGroups[i].interactable = (i == 0);
            quadrinhosCanvasGroups[i].blocksRaycasts = (i == 0);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
    }

    // Método para iniciar a história em quadrinhos
    public void IniciarQuadrinhos()
    {
        indiceAtual = 0;
        StartCoroutine(IniciarSequenciaQuadrinhos());
    }

    private IEnumerator IniciarSequenciaQuadrinhos()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(quadrinhosCanvasGroups[indiceAtual]));
    }

    private void Update()
    {
        // Avança para o próximo quadrinho ao pressionar a tecla F
        if (Input.GetButtonDown("Interagir") && indiceAtual < quadrinhosCanvasGroups.Count - 1)
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
        else if (Input.GetButtonDown("Interagir") && indiceAtual == quadrinhosCanvasGroups.Count - 1)
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
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        quadrinhos.SetActive(false);
        animatorFade.SetTrigger("abrir");
        creditos.SetTrigger("comecar");
        animator.SetActive(false);
        sair.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
