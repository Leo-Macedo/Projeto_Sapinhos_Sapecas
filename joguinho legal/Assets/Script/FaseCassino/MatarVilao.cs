using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MatarVilao : MonoBehaviour
{

    private Animator animator;

    //Melo vilão
    public Animator animatorvilao;
    public GameObject vilao;
    public Transform posicaovilao;
    public NavMeshAgent agentvilao;

    //Matar Melo
    private bool rPressed = true;
    private bool tPressed = true;


    public bool podeatacar = true;
    public float distAtaque;
    public int vidavilao = 3;
   

    //ScriptVidaVilao
    private VidaVilao vidaVilao;
    public bool socoExecutado = false; 
    

    void Start()
    {
        //Referencia os componentes
        animator = GetComponent<Animator>();
        animatorvilao = vilao.GetComponent<Animator>();
        vidaVilao = vilao.GetComponent<VidaVilao>();
        agentvilao = vilao.GetComponent<NavMeshAgent>();


    }

    void Update()
    {    //Atacar e dar dano no Vilao
        if (Input.GetKeyDown(KeyCode.R))
        {
            rPressed = false;
            Invoke("ResetaGolpe", 1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tPressed = false;
            Invoke("ResetaGolpe", 1);

        }
        DarDanoNoVilao();

    }

   public void DarDanoNoVilao()
{
    if (socoExecutado)  // Executa o código apenas se o evento foi acionado pela animação
    {
        float distancia = Vector3.Distance(transform.position, posicaovilao.position);
        if (distancia <= distAtaque && podeatacar)
        {
            Debug.Log("Chamo evento pela animação");
            vidaVilao.ReceberDanoVilao(1);
            animatorvilao.SetBool("caiu", true);
            Invoke("Resetou", 2);
            rPressed = true;
            tPressed = true;
            podeatacar = false;
            Invoke("PodeAtacar", 3);
        }

        socoExecutado = false;  // Reseta a variável para evitar dano contínuo
    }
}

// Esta função deve ser chamada pelo AnimationEvent
public void AcionarSoco()
{
    socoExecutado = true;  // Ativa o trigger quando a animação de soco acontece
}

    //Resetar animação de tomar soco
    public void Resetou()
    {
        animatorvilao.SetBool("caiu", false);
    }

    //Resetar para pode atacar novamente
    public void PodeAtacar()
    {
        podeatacar = true;
    }

    public void ResetaGolpe()
    {
         rPressed = true;
         tPressed = true;
    }

}
