using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PassarLabirinto : MonoBehaviour
{
    public GameObject txtCorreto;
    public GameObject txtErrado;
    public AudioSource[] som;
    private int contador = 0;
    public Transform[] waypoints;
    private Transform player;
    public Animator animatorFade;
    private bool flag;
    private Movimento2 movimento2;
    private float veloAndandoInicial;
    private float veloCorrendoInicial;
    public PlayableDirector cutscene;

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject[] mensagem;
    public Animator[] animatorMSG;

    void Start()
    {
        player = GetComponent<Transform>();
        movimento2 = GetComponent<Movimento2>();
        veloAndandoInicial = movimento2.veloAndando;
        veloCorrendoInicial = movimento2.veloCorrendo;
        StartCoroutine(ControlarMovimentoDuranteCutscene());
    }

    void Update() { }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("certo") && !flag)
        {
            contador++;
            Debug.Log("Caminho certo: 0" + contador);

            VerificarLabirinto();
            flag = true;
        }
        if (other.gameObject.CompareTag("errado") && !flag)
        {
            contador = 0;
            Debug.Log("Caminho errado: " + contador);

            VerificarLabirinto();
            flag = true;
        }
    }

    private void VerificarLabirinto()
    {
        switch (contador)
        {
            case 0:
                StartCoroutine(TP(0));
                break;

            case 1:
                StartCoroutine(TP(1));
                break;

            case 2:
                StartCoroutine(TP(2));
                break;

            case 3:
                StartCoroutine(TP(3));
                break;

            case 4:
                StartCoroutine(TP(4));
                break;
            case 5:
                StartCoroutine(TrocarCena());
                break;
        }
    }

    public IEnumerator TP(int num)
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }
        yield return new WaitForSeconds(2);
        player.position = waypoints[num].position;
        player.rotation = Quaternion.Euler(0, waypoints[num].rotation.eulerAngles.y, 0); // Define apenas a rotação no eixo Y
        yield return new WaitForSeconds(0.1f);
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }

        flag = false;
        yield return new WaitForSeconds(0.5f);
        if (num == 0)
        {
            txtErrado.SetActive(true);
            som[1].Play();
        }
        else
        {
            txtCorreto.SetActive(true);
            som[0].Play();
        }

        yield return new WaitForSeconds(3f);

        txtErrado.SetActive(false);
        txtCorreto.SetActive(false);
    }

    public IEnumerator TrocarCena()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Cassino");
    }

    private IEnumerator ControlarMovimentoDuranteCutscene()
    {
        // Zera a velocidade
        movimento2.veloAndando = 0f;
        movimento2.veloCorrendo = 0f;
        yield return new WaitForSeconds(0.5f);

        // Espera pela duração da cutscene
        yield return new WaitForSeconds((float)cutscene.duration);

        // Restaura as velocidades iniciais
        movimento2.veloAndando = veloAndandoInicial;
        movimento2.veloCorrendo = veloCorrendoInicial;
        somNoti.Play();
        mensagem[0].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[0].SetTrigger("fechou");
        somNoti.Play();
        mensagem[1].SetActive(true);
        yield return new WaitForSeconds(5);
        animatorMSG[1].SetTrigger("fechou");
    }
}
