using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperSystem : MonoBehaviour
{
    [Header("Super Settings")]
    public float chargeTime = 10f; // Tempo para carregar o super
    public float superDuration = 5f; // Duração do super
    public Slider superSlider; // Referência ao Slider UI

    public Image slider;
    public Color corSuper;
    private Color corInicial;

    [Header("Romarinho")]
    public GameObject romarinho;
    public ParticleSystem particulas;
    private Movimento2 movimento2;
    private MatarVilao matarVilao;
    private Transform transformRomarinho;

    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private bool isSuperReady = false;
    private bool isSuperActive = false;

    private float originalDistAtaque;

    void Start()
    {
        movimento2 = GetComponent<Movimento2>();
        matarVilao = GetComponent<MatarVilao>();
        transformRomarinho = GetComponent<Transform>();
        corInicial = slider.color;

        StartChargingSuper();

        // Inicializa o slider
        superSlider.maxValue = chargeTime;
        superSlider.value = 0f;

        originalDistAtaque = matarVilao.distAtaque;
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

        // Verifica se o super está pronto e a tecla "I" foi pressionada
        if (isSuperReady && Input.GetKeyDown(KeyCode.R))
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

    // Método para começar a carregar o super
    public void StartChargingSuper()
    {
        isCharging = true;
        currentChargeTime = 0f;
        isSuperReady = false;
        isSuperActive = false;
    }

    // Método para ativar o super
    private void ActivateSuper()
    {
        if (isSuperReady)
        {
            Debug.Log("Super ativado!");
            //referencias variaveis

            movimento2.veloAndando *= 4;
            movimento2.veloCorrendo *= 4;
            matarVilao.danoAtaque *= 2;
            transformRomarinho.localScale *= 1.5f;
            matarVilao.distAtaque = 4f;
            particulas.Play();

            isSuperActive = true;
            isSuperReady = false;
        }
    }

    // Método para desativar o super
    private void DeactivateSuper()
    {
        Debug.Log("Super desativado!");
        isSuperActive = false;
        currentChargeTime = 0f;
        superSlider.value = 0f;
        slider.color = corInicial;
        superSlider.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

        matarVilao.distAtaque = originalDistAtaque;
        particulas.Stop();

        // Resetando os valores ao desativar o super
        movimento2.veloAndando /= 4;
        movimento2.veloCorrendo /= 4;
        matarVilao.danoAtaque /= 2;
        transformRomarinho.localScale /= 1.5f;
        StartChargingSuper();
    }
}
