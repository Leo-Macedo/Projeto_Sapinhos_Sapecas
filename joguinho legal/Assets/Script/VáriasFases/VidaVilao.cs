using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class VidaVilao : MonoBehaviour
{
    // Vida do vilão (campo privado)
    private int _vidavilao;

    // Propriedade pública para acessar e modificar a vida do vilão
    public int Vida
    {
        get { return _vidavilao; }
        set
        {
            _vidavilao = Mathf.Max(0, value); // Garante que a vida não seja negativa
            slidervidavilao.value = _vidavilao; // Atualiza o slider
            Debug.Log("Vida do Vilão: " + _vidavilao);

            if (_vidavilao <= 0)
            {
                VilaoMorreu();
            }
        }
    }

    //Vida e o slider dela
    public Slider slidervidavilao;
    private Animator animatorvilao;
    private NavMeshAgent agentvilao;
    public GameObject portalvoltar;
    private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        agentvilao = GetComponent<NavMeshAgent>();

        // Configura o slider para o valor máximo da vida do vilão
        // Inicializa a vida do vilão
        Vida = 5; // Defina o valor inicial da vida aqui
        slidervidavilao.maxValue = Vida;
        slidervidavilao.value = Vida;
    }

    //Receber dano e morrer
    public void ReceberDanoVilao(int dano)
    {
        Vida -= dano;
        slidervidavilao.value = Vida;

        if (vidaPersonagem.acabouojogo)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void VilaoMorreu()
    {
        if (agentvilao != null)
        {
            agentvilao.isStopped = true;
            agentvilao.speed = 0f;
        }

        animatorvilao.SetBool("caiu", false);
        animatorvilao.SetBool("nocaute", true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        vidaPersonagem.acabouojogo = true;
        txtganhou.SetActive(true);
        portalvoltar.SetActive(true);
    }
}
