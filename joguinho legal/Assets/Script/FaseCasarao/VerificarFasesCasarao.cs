using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class VerificarFasesCasarao : MonoBehaviour
{
    public int controladorFases = 0;
    public GameObject cubo;
    public LançarObjetoEMoverOlivia lançarObjeto;

    [Header("Hud")]
    public GameObject gameOverCanvas;

    [Header("CutScenes")]
    public PlayableDirector cutscenePorao;
    public PlayableDirector cutsceneEntradaPrincipal;
    public PlayableDirector cutsceneSegundoAndar;
    public PlayableDirector cutsceneSotao;

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject[] mensagem;
    public Animator[] animatorMSG;

    private VidaPersonagem vidaPersonagem;
    public Movimento2[] movimento2;
    private float veloAndandoInicial;
    private float veloCorrendoInicial;

    public Animator animatorFade;
    public CinemachineFreeLook freeLookCamera;
    public GameObject rivaldo;
    public GameObject romario;
    public GameObject ronaldo;
    public GameObject[] canvas;
    public Transform waiPoint;

    void Start()
    {
        vidaPersonagem = ronaldo.GetComponent<VidaPersonagem>();
        controladorFases = PlayerPrefs.GetInt("ControladorFasesCasarao", 0);

        if (movimento2.Length > 0)
        {
            veloAndandoInicial = movimento2[0].veloAndando;
            veloCorrendoInicial = movimento2[0].veloCorrendo;
        }

        foreach (Movimento2 movimento in movimento2)
        {
            // Aqui você pode acessar cada elemento
            Debug.Log(
                $"Velocidade Andando: {movimento.veloAndando}, Velocidade Correndo: {movimento.veloCorrendo}"
            );
        }

        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no meio da tela
        Cursor.visible = false; // Torna o cursor invisível
        CarregarRound();
    }

    void Update()
    {
        //VerificarMorteJogador();
    }

    void CarregarRound()
    {
        // Define a posição do player com base no controlador de fases
        switch (controladorFases)
        {
            case 0:
                StartCoroutine(Porao());
                break;

            case 1:
                StartCoroutine(EntradaPrincipal());
                break;

            case 2:
                StartCoroutine(SegundoAndar());
                break;

            case 3:
                StartCoroutine(Sotao());
                break;
            default:
                Debug.LogWarning("Controlador de fases desconhecido: " + controladorFases);
                break;
        }
    }

    /* private void VerificarMorteJogador()
     {
         if (vidaPersonagem.vidaAtual <= 0)
         {
             Time.timeScale = 0f; // Pausa o jogo
             gameOverCanvas.SetActive(true);
             Cursor.lockState = CursorLockMode.None;
             Cursor.visible = true;
         }
     } */

    public IEnumerator Porao()
    {
        cutscenePorao.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        yield return new WaitForSeconds((float)cutscenePorao.duration);
        cubo.SetActive(false);
        somNoti.Play();
        mensagem[0].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[0].SetTrigger("fechou");
        somNoti.Play();
        mensagem[1].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[1].SetTrigger("fechou");
    }

    public IEnumerator EntradaPrincipal()
    {
        if (canvas != null && canvas.Length > 1)
        {
            if (canvas[0] != null)
                canvas[0].SetActive(false);
            if (canvas[1] != null)
                canvas[1].SetActive(true);
        }

        if (rivaldo != null)
        {
            rivaldo.SetActive(true);
        }
        if (romario != null)
        {
            romario.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }
        cutsceneEntradaPrincipal.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        yield return new WaitForSeconds((float)cutsceneEntradaPrincipal.duration);
        somNoti.Play();
        mensagem[2].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[2].SetTrigger("fechou");
        somNoti.Play();
        mensagem[3].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[3].SetTrigger("fechou");
    }

    public IEnumerator SegundoAndar()
    {
       
        cutsceneSegundoAndar.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        yield return new WaitForSeconds((float)cutsceneSegundoAndar.duration);
        somNoti.Play();
        mensagem[5].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[5].SetTrigger("fechou");
        somNoti.Play();
        mensagem[8].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[8].SetTrigger("fechou");
    }

    public IEnumerator Sotao()
    {
        if (ronaldo != null && waiPoint != null)
        {
            ronaldo.transform.position = waiPoint.position;
            ronaldo.transform.rotation = waiPoint.rotation;
        }

        yield return new WaitForSeconds(0.1f);
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }
        cutsceneSotao.Play();
        StartCoroutine(ControlarMovimentoDuranteCutscene());
        yield return new WaitForSeconds((float)cutsceneSotao.duration);
        lançarObjeto.podeLancar = true;
        somNoti.Play();
        mensagem[6].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[6].SetTrigger("fechou");
        somNoti.Play();
        mensagem[7].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[7].SetTrigger("fechou");
    }

    public IEnumerator PassouFasePorao()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);

        StartCoroutine(EntradaPrincipal());

        Debug.Log("funcionouuuuuuuuuuuuuuuuuuuuuuu");
    }

    public IEnumerator PassouFaseEntradaPrincipal()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Casarao2");
    }

    public IEnumerator PassouFaseSegundoAndar()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);

        StartCoroutine(Sotao());
    }

    private IEnumerator ControlarMovimentoDuranteCutscene()
    {
        foreach (Movimento2 movimento in movimento2)
        {
            movimento.veloAndando = 0f;
            movimento.veloCorrendo = 0f;
        }

        yield return new WaitForSeconds((float)cutscenePorao.duration);

        foreach (Movimento2 movimento in movimento2)
        {
            movimento.veloAndando = veloAndandoInicial;
            movimento.veloCorrendo = veloCorrendoInicial;
        }
    }

    public void AtualizarControladorFases()
    {
        controladorFases += 1;
        PlayerPrefs.SetInt("ControladorFasesCasarao", controladorFases);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = " + controladorFases);
    }
}
