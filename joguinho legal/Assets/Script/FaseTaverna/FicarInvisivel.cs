using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FicarInvisivel : MonoBehaviour
{
    public AudioSource somIA;
    [Header("Super Settings")]
    public float chargeTime = 10f; // Tempo para carregar o super
    public float superDuration = 5f; // Duração do super
    public Slider superSlider; // Referência ao Slider UI
    public Transform ponto;

    public Image slider;
    public Color corSuper;
    private Color corInicial;

    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private bool isSuperReady = false;
    private bool isSuperActive = false;

    public bool isInvisible = false;
    public GameObject objetoInvisivel; // O objeto a ser ativado e desativado
    public GameObject objetoColorido; // O objeto colorido a ser desativado

    void Start()
    {
        StartChargingSuper();
        corInicial = slider.color;

        // Inicializa o slider
        superSlider.maxValue = chargeTime;
        superSlider.value = 0f;
    }

    private void Update()
    {
        CarregarSuper();
    }

    public void StartChargingSuper()
    {
        isCharging = true;
        currentChargeTime = 0f;
        isSuperReady = false;
        isSuperActive = false;
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

    private void ActivateSuper()
    {
        if (isSuperReady)
        {
            somIA.Play();
            isInvisible = true;
            isSuperActive = true; // Super foi ativado
            isSuperReady = false; // Não pode ativar novamente até recarregar
            SetVisibility(isInvisible);
            GameObject prefab = Resources.Load<GameObject>("anfibia");
            Instantiate(prefab, ponto.position, ponto.rotation);// Torna o objeto invisível
        }
    }

    private void DeactivateSuper()
    {
        isInvisible = false;
        isSuperActive = false;
        slider.color = corInicial;
        superSlider.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        // Super foi desativado
        currentChargeTime = 0f;
        superSlider.value = 0f;
        StartChargingSuper();
        SetVisibility(isInvisible);
    }

    private void SetVisibility(bool invisible)
    {
        if (objetoInvisivel != null)
        {
            objetoInvisivel.SetActive(invisible);
        }

        if (objetoColorido != null)
        {
            objetoColorido.SetActive(!invisible);
        }
    }
}
