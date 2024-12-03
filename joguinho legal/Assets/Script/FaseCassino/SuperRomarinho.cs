using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperSystem : MonoBehaviour
{
    public bool podeUtar;
    [Header("Super Settings")]
    public float chargeTime = 10f; // Tempo para carregar o super
    public float superDuration = 5f; // Duração do super
    public Slider superSlider; // Referência ao Slider UI

    public Image slider;
    public Color corSuper;
    private Color corInicial;

    [Header("Romarinho")]
    public GameObject romarinho;
    private MatarVilao matarVilao;

    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private bool isSuperReady = false;
    public bool isSuperActive = false;

    private float originalDistAtaque;

    public GameObject romarioNormal;
    public GameObject romarioSuper;

    public VidaPersonagem vidaRomarioNormal;
    public VidaPersonagem vidaRomarioSuper;
    public VilaoSegueEAtaca vilaoSegueEAtaca;
    public VidaVilao vidaVilao;
    public VilaoAtacaPlayer vilaoAtacaPlayer;

    void Start()
    {
        matarVilao = GetComponent<MatarVilao>();
        corInicial = slider.color;

        StartChargingSuper();

        // Inicializa o slider
        superSlider.maxValue = chargeTime;
        superSlider.value = 0f;

        // Garante que apenas o romarioNormal esteja ativo no início
        romarioNormal.SetActive(true);
        romarioSuper.SetActive(false);

        // Referência ao vilão
    }

    void Update()
    {
        CarregarSuper();
    }

    private void CarregarSuper()
    {
        if (isCharging)
        {
            // Incrementa o tempo de carregamento
            currentChargeTime += Time.deltaTime;
            superSlider.value = currentChargeTime;

            // Se o tempo de carregamento atingir o tempo necessário, o super está pronto
            if (currentChargeTime >= chargeTime)
            {
                isCharging = false;
                isSuperReady = true;
                slider.color = corSuper;
                superSlider.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

                Debug.Log("Super carregado e pronto para ativar!");
            }
        }

        // Verifica se o super está pronto e a tecla "R" foi pressionada
        if (isSuperReady && Input.GetKeyDown(KeyCode.R) && !podeUtar)
        {
            ActivateSuper();
        }

        // Verifica se o super está ativo e diminui o valor do slider
        if (isSuperActive)
        {
            currentChargeTime -= (chargeTime / superDuration) * Time.deltaTime;
            superSlider.value = currentChargeTime;

            // Se o tempo acabar, desative o super
            if (currentChargeTime <= 0f)
            {
                DeactivateSuper();
            }
        }
    }

    public void StartChargingSuper()
    {
        isCharging = true;
        currentChargeTime = 0f;
        isSuperReady = false;
        isSuperActive = false;
    }

    private void ActivateSuper()
    {
        if (isSuperReady)
        {
            Debug.Log("Super ativado!");

            GameObject somChefeMorreu = GameObject.FindWithTag("somsapocalipse");
            if (somChefeMorreu != null)
            {
                AudioSource audioSource = somChefeMorreu.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }

            // Sincroniza posição e rotação
            Vector3 currentPosition = romarioNormal.transform.position;
            Quaternion currentRotation = romarioNormal.transform.rotation;

            romarioSuper.transform.position = currentPosition;
            romarioSuper.transform.rotation = currentRotation;

            // Sincroniza a vida
            vidaRomarioSuper.vidaAtual = vidaRomarioNormal.vidaAtual;

            romarioNormal.SetActive(false);
            romarioSuper.SetActive(true);

            if (vilaoSegueEAtaca != null)
            {
                vilaoSegueEAtaca.player = romarioSuper.transform;
                vilaoSegueEAtaca.vidaPersonagemScript = romarioSuper.GetComponent<VidaPersonagem>();
            }

            if (vidaVilao != null)
            {
                vidaVilao.player = romarioSuper;
                vidaVilao.vidaPersonagem = romarioSuper.GetComponent<VidaPersonagem>();
            }

            if (vilaoAtacaPlayer != null)
            {
                vilaoAtacaPlayer.player = romarioSuper.transform;
                vilaoAtacaPlayer.vidaPersonagemScript = romarioSuper.GetComponent<VidaPersonagem>();
                vilaoAtacaPlayer.açõesPersonagem = romarioSuper.GetComponent<AçõesPersonagem>();
            }

            isSuperActive = true;
            isSuperReady = false;
        }
    }

    private void DeactivateSuper()
    {
        Debug.Log("Super desativado!");
        isSuperActive = false;
        currentChargeTime = 0f;
        superSlider.value = 0f;
        slider.color = corInicial;
        superSlider.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

        // Sincroniza posição e rotação
        Vector3 currentPosition = romarioSuper.transform.position;
        Quaternion currentRotation = romarioSuper.transform.rotation;

        romarioNormal.transform.position = currentPosition;
        romarioNormal.transform.rotation = currentRotation;

        // Sincroniza a vida
        vidaRomarioNormal.vidaAtual = vidaRomarioSuper.vidaAtual;

        romarioSuper.SetActive(false);
        romarioNormal.SetActive(true);

        if (vilaoSegueEAtaca != null)
        {
            vilaoSegueEAtaca.player = romarioNormal.transform;
            vilaoSegueEAtaca.vidaPersonagemScript = romarioNormal.GetComponent<VidaPersonagem>();
        }

        if (vidaVilao != null)
        {
            vidaVilao.player = romarioNormal;
            vidaVilao.vidaPersonagem = romarioNormal.GetComponent<VidaPersonagem>();
        }

        if (vilaoAtacaPlayer != null)
        {
            vilaoAtacaPlayer.player = romarioNormal.transform;
            vilaoAtacaPlayer.vidaPersonagemScript = romarioNormal.GetComponent<VidaPersonagem>();
            vilaoAtacaPlayer.açõesPersonagem = romarioNormal.GetComponent<AçõesPersonagem>();
        }

        StartChargingSuper();
    }
}
