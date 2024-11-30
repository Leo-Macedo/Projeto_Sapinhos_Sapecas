using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class VidaVilao : MonoBehaviour
{
    // Vida do vilão (campo privado)
    [SerializeField]
    private int _vidaVilao;
    public AudioSource somDor;

    // Propriedade pública para acessar e modificar a vida do vilão
    public int Vida
    {
        get { return _vidaVilao; }
        set
        {
            _vidaVilao = Mathf.Max(0, value);
            slidervidavilao.value = _vidaVilao;
            Debug.Log("Vida do Vilão: " + _vidaVilao);

            if (_vidaVilao <= 0)
            {
                VilaoMorreu();
            }
        }
    }

    // Vida e o slider dela
    public Slider slidervidavilao;
    private Animator animatorvilao;
    private NavMeshAgent agentvilao;
    private Rigidbody rb;

    // Script VidaPersonagem
    public GameObject player;
    public VidaPersonagem vidaPersonagem;
    public bool morreuvilao = false;
    public int VidaInicial;

    public bool edmundo;
    public Transform ponto;

    void Start()
    {
        // Pegar animator e outros componentes
        animatorvilao = GetComponent<Animator>();
        agentvilao = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        VidaInicial = Vida;
        // Inicializa a vida do vilão e configura o slider
        slidervidavilao.maxValue = Vida;
        slidervidavilao.value = Vida;
    }

    // Receber dano e atualizar o slider
    public void ReceberDanoVilao(int dano)
    {
        Vida -= dano;
        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", true);
        }
        Invoke("Resetou", 2);
        somDor.Play();

        if (edmundo)
        {
            GameObject prefab = Resources.Load<GameObject>("GosmaEdmundo");
            Instantiate(prefab, ponto.position, Quaternion.identity);
        }

        if (vidaPersonagem.vidaAtual <= 0)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Lógica quando o vilão morre
    public void VilaoMorreu()
    {
        if (agentvilao != null)
        {
            agentvilao.isStopped = true;
            agentvilao.speed = 0f;
        }

        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", false);
            animatorvilao.SetBool("nocaute", true);
        }
        rb.constraints = RigidbodyConstraints.FreezeAll;
        morreuvilao = true;

        if (!edmundo)
        {
            GameObject somChefeMorreu = GameObject.FindWithTag("somchefemorreu");
            if (somChefeMorreu != null)
            {
                AudioSource audioSource = somChefeMorreu.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }
    }

    public void Resetou()
    {
        if (animatorvilao != null)
        {
            animatorvilao.SetBool("caiu", false);
        }
    }
}
