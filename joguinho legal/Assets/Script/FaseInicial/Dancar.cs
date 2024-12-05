using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dancar : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;
    public List<Button> botoes;
    private Animator animator;
    public GameObject painel;
    public AudioClip[] musicas;
    public AudioSource musicaAudio;

    private bool isDancando = false;  // Flag para verificar se está dançando

    void Start()
    {
        animator = GetComponent<Animator>();

        for (int i = 0; i < botoes.Count; i++)
        {
            int indice = i + 1;
            botoes[i].onClick.AddListener(() => StartCoroutine(DancarAnim(indice)));
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Dancar"))
        {
            painel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            controleSensibilidadeCamera.podePausar = false;
        }

        float inputX = animator.GetFloat("inputX");
        float inputY = animator.GetFloat("inputY");

        // Se o jogador começar a andar, a dança será interrompida
        if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputY) > 0)
        {
            PararMusica();
        }
    }

    public IEnumerator DancarAnim(int num)
    {
        if (isDancando)
        {
            PararMusica();
            yield return new WaitForSeconds(0.1f);  
        }

        isDancando = true;  

        animator.SetInteger("dancar", num);  

        if (num >= 0 && num < musicas.Length)
        {
            musicaAudio.clip = musicas[num];
            musicaAudio.Play();  
        }

        painel.SetActive(false);  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controleSensibilidadeCamera.podePausar = true;
    }

    

    public void PararMusica()
    {
        animator.SetInteger("dancar", 0);  

        if (musicaAudio.isPlaying)
        {
            musicaAudio.Stop();  
        }

        isDancando = false;  
    }
}
