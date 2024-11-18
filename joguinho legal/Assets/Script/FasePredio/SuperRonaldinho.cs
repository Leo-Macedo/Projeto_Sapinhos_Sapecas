using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SuperRonaldinho : MonoBehaviour
{
    [Header("Super Settings")]
    public float chargeTime = 10f; // Tempo para carregar o super
    public Slider superSlider; // Referência ao Slider UI
    public Image slider; // Imagem da barra do slider
    public Color corSuper; // Cor do slider quando o super está pronto
    private Color corInicial; // Cor inicial do slider

    [Header("Ronaldinho")]
    public GameObject ronaldinho; // Alterado de romarinho para ronaldinho
    public ParticleSystem particulas;
    public float multiplicadorSuper = 2f; // Multiplicador para o super pulo
    public float forçaEmpurraoCapanga = 5f; // Força aplicada aos capangas
    public float raioDetectarCapangas = 5f; // Raio para detectar capangas ao redor

    private Transform transformRonaldinho;
    private Rigidbody rb;
    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private bool isSuperReady = false;
    public bool isSuperActive = false;
    public bool podeUtar;

    public int camadaRonaldinho;
    public int camadaCapangas;

    [Header("Super Jump Forces")]
    public float forcaCima = 5f; // Força para cima
    public float forcaTras = 5f; // Força para trás

    void Start()
    {
        rb = ronaldinho.GetComponent<Rigidbody>();
        transformRonaldinho = ronaldinho.GetComponent<Transform>();
        corInicial = slider.color; // Guarda a cor inicial do slider

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
            currentChargeTime += Time.deltaTime;
            superSlider.value = currentChargeTime;

            if (currentChargeTime >= chargeTime)
            {
                isCharging = false;
                isSuperReady = true;
                slider.color = corSuper;
                superSlider.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                Debug.Log("Super carregado e pronto para ativar!");
            }
        }

        if (isSuperReady && Input.GetKeyDown(KeyCode.R) && podeUtar)
        {
            ActivateSuper();
        }

        if (isSuperActive)
        {
            currentChargeTime = 0f;
            superSlider.value = 0f;

            Invoke("DeactivateSuper", 3);
        }
    }

    public void StartChargingSuper()
    {
        isCharging = true;
        currentChargeTime = 0f;
        superSlider.value = currentChargeTime;
        isSuperReady = false;
        isSuperActive = false;
        slider.color = corInicial;
    }

    private void ActivateSuper()
    {
        if (isSuperReady)
        {
            Debug.Log("Super ativado!");
            slider.color = corInicial;
            superSlider.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            isSuperActive = true;
            isSuperReady = false;

            Physics.IgnoreLayerCollision(camadaRonaldinho, camadaCapangas, true);

            Pular(multiplicadorSuper);

            if (particulas != null)
            {
                particulas.Play();
            }
        }
    }

    private void DeactivateSuper()
    {
        Debug.Log("Super desativado!");
        isSuperActive = false;
        particulas.Stop();

        Physics.IgnoreLayerCollision(camadaRonaldinho, camadaCapangas, false);

        StartChargingSuper();
    }

    private void Pular(float multiplicador = 1f)
    {
        rb.AddForce(Vector3.up * 10f * multiplicador, ForceMode.Impulse);
        Debug.Log("Pulo ativado com multiplicador: " + multiplicador);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao") && isSuperActive)
        {
            StartCoroutine(AplicarForcaCapangas());
        }
    }

    private IEnumerator AplicarForcaCapangas()
    {
        yield return new WaitForSeconds(0.1f);

        Collider[] capangas = Physics.OverlapSphere(
            transformRonaldinho.position,
            raioDetectarCapangas
        );
        foreach (var col in capangas)
        {
            if (col.CompareTag("capanga"))
            {
                NavMeshAgent agent = col.GetComponent<NavMeshAgent>();
                Rigidbody capangaRb = col.GetComponent<Rigidbody>();

                if (agent != null)
                {
                    agent.enabled = false; // Desativa temporariamente o NavMeshAgent
                }

                if (capangaRb != null)
                {
                    Vector3 direcaoParaTras = (
                        col.transform.position - transformRonaldinho.position
                    ).normalized;
                    Vector3 forcaTotal = (direcaoParaTras * forcaTras) + (Vector3.up * forcaCima);

                    capangaRb.AddForce(forcaTotal, ForceMode.Impulse);
                    Debug.Log("Capanga empurrado para trás e para cima com força diagonal!");
                }

                if (agent != null)
                {
                    StartCoroutine(ReativarNavMeshAgent(agent));
                }
            }
        }
    }

    private IEnumerator ReativarNavMeshAgent(NavMeshAgent agent)
    {
        yield return new WaitForSeconds(3f); // Tempo para reativar o NavMeshAgent após o super
        agent.enabled = true;
    }
}
