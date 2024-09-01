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

    // Propriedade pública para acessar e modificar a vida do vilão
    public int Vida
    {
        get { return _vidaVilao; }
        set
        {
            _vidaVilao = Mathf.Max(0, value); // Garante que a vida não seja negativa
            slidervidavilao.value = _vidaVilao; // Atualiza o slider
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
    private VidaPersonagem vidaPersonagem;
    public bool morreuvilao = false;
    public int VidaInicial;


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
        animatorvilao.SetBool("caiu", true);
        Invoke("Resetou", 2);

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

        animatorvilao.SetBool("caiu", false);
        animatorvilao.SetBool("nocaute", true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Resetou()
    {
        animatorvilao.SetBool("caiu", false);
    }
}
