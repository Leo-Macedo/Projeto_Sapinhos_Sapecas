using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;


public class VidaVilao : MonoBehaviour
{
     //Vida e o slider dela
    public int vidavilao;
    public Slider slidervidavilao;
    private Animator animatorvilao;
    private NavMeshAgent agentvilao;
    public GameObject portalvoltar;

    //Script VidaPersonagem
    public GameObject player;
    private VidaPersonagem vidaPersonagem;
    public bool morreuvilao = false;
    public GameObject txtganhou;
    void Start()
    {
        //Pegar animator
        agentvilao = GetComponent<NavMeshAgent>();
        animatorvilao = GetComponent<Animator>();

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        agentvilao = GetComponent<NavMeshAgent>();
    }

    //Receber dano e morrer
    public void ReceberDanoVilao(int dano)
    {
        vidavilao -= dano;
        slidervidavilao.value = vidavilao;
        Debug.Log("Vida do Vilão: " + vidavilao);

        if (vidavilao <= 0)
        {
            animatorvilao.SetBool("caiu", false);
            animatorvilao.SetBool("nocaute", true);
            agentvilao.isStopped = true;
            agentvilao.speed = 0f;
            vidaPersonagem.acabouojogo = true;
            txtganhou.SetActive(true);
            portalvoltar.SetActive(true);
        }
        if(vidaPersonagem.acabouojogo)
        {
            agentvilao.isStopped = true;
            agentvilao.speed = 0f;
            
        }
    }
}
