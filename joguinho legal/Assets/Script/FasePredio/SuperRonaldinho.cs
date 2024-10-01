using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperRonaldinho : MonoBehaviour
{
    [Header("Super Settings")]
    public float chargeTime = 10f; // Tempo para carregar o super
    public Slider superSlider; // Referência ao Slider UI

    [Header("Ronaldinho")]
    public GameObject ronaldinho; // Alterado de romarinho para ronaldinho
    public ParticleSystem particulas;
    public float multiplicadorSuper = 2f; // Multiplicador para o super pulo
    public float forçaEmpurraoCapanga = 5f; // Força aplicada aos capangas
    public float raioDetectarCapangas = 5f; // Raio para detectar capangas ao redor

    private Transform transformRonaldinho; // Alterado para pegar o Transform de ronaldinho
    private Rigidbody rb;
    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private bool isSuperReady = false;
    public bool isSuperActive = false;

    void Start()
    {
        rb = ronaldinho.GetComponent<Rigidbody>();
        transformRonaldinho = ronaldinho.GetComponent<Transform>(); // Alterado para pegar o Transform de ronaldinho

        StartChargingSuper();

        // Inicializa o slider
        superSlider.maxValue = chargeTime;
        superSlider.value = 0f;
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
                Debug.Log("Super carregado e pronto para ativar!");
            }
        }

        // Verifica se o super está pronto e a tecla "I" foi pressionada
        if (isSuperReady && Input.GetKeyDown(KeyCode.R))
        {
            ActivateSuper();
        }

        // Verifica se o super está ativo e reinicia o slider
        if (isSuperActive)
        {
            // O super pulo deve descarregar completamente de uma vez, sem duração
            // Resetamos o slider e o tempo de carregamento
            currentChargeTime = 0f;
            superSlider.value = 0f;

            Invoke("DeactivateSuper", 3); // Desativa o super após usar
        }
    }

    // Método para começar a carregar o super
    public void StartChargingSuper()
    {
        isCharging = true;
        currentChargeTime = 0f;
        superSlider.value = currentChargeTime; // Reinicia o valor do slider
        isSuperReady = false;
        isSuperActive = false;
    }

    // Método para ativar o super
    private void ActivateSuper()
    {
        if (isSuperReady)
        {
            Debug.Log("Super ativado!");

            isSuperActive = true;
            isSuperReady = false;

            // Ativa o super pulo
            Pular(multiplicadorSuper);

            // Adiciona partículas ou qualquer outro efeito visual
            if (particulas != null)
            {
                particulas.Play();
            }
        }
    }

    // Método para desativar o super
    private void DeactivateSuper()
    {
        Debug.Log("Super desativado!");
        isSuperActive = false;
        particulas.Stop();
        StartChargingSuper(); // Começa a carregar novamente
    }

    // Novo método para pular com multiplicador
    private void Pular(float multiplicador = 1f)
    {
        // Aplica a força de pulo com o multiplicador
        rb.AddForce(Vector3.up * 10f * multiplicador, ForceMode.Impulse); // Ajuste a força de pulo conforme necessário
        Debug.Log("Pulo ativado com multiplicador: " + multiplicador);
    }

    // Detecta colisão com o chão
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se a colisão foi com o chão
        if (collision.gameObject.CompareTag("Chao") && isSuperActive)
        {
            StartCoroutine(AplicarForcaCapangas());
        }
    }

    // Coroutine para aplicar força aos capangas
    private IEnumerator AplicarForcaCapangas()
    {
        // Aguarda um pouco para garantir que o jogador tenha tempo de cair no chão
        yield return new WaitForSeconds(0.1f);

        // Obtém todos os colliders ao redor do jogador
        Collider[] capangas = Physics.OverlapSphere(transformRonaldinho.position, raioDetectarCapangas);
        foreach (var col in capangas)
        {
            if (col.CompareTag("capanga"))
            {
                Rigidbody capangaRb = col.GetComponent<Rigidbody>();
                if (capangaRb != null)
                {
                    // Aplica uma força para trás aos capangas
                    Vector3 direcaoParaTras = (col.transform.position - transformRonaldinho.position).normalized;
                    capangaRb.AddForce(direcaoParaTras * forçaEmpurraoCapanga, ForceMode.Impulse);
                    Debug.Log("Capanga empurrado para trás!");
                }
            }
        }
    }
}
