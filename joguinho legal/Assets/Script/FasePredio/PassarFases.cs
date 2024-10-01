using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PassarFases : MonoBehaviour
{
    private List<GameObject> capangasAndar2 = new List<GameObject>(); // Lista para armazenar os capangas do andar 2
    private List<GameObject> capangasAndar3 = new List<GameObject>(); // Lista para armazenar os capangas do andar 3
    private List<GameObject> capangasAndar4 = new List<GameObject>(); // Lista para armazenar os capangas do andar 4

    private Transform player;
    public Transform andar2;
    public Transform andar3;
    public Transform andar4;
    public Animator animatorEscada;
    public VerificarFasePredio verificarFasePredio;
    private VidaPersonagem vidaPersonagem;

    public bool boolandar2 = false;
    public bool boolandar3 = false;
    public bool boolandar4 = false;

     [Header("Hud")]
    public GameObject vidaMelo;
    public GameObject vidaPlayer;
    public GameObject Super;

    [Header("Cutscenes")]
    private bool tocouCutScene3;
    private bool tocouCutScene4;
    public PlayableDirector cutsceneSubiuParkour1;
    public PlayableDirector cutsceneDesceuParkour1;
    public PlayableDirector cutsceneSubiuParkour2;

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject mensagem7;
    public GameObject mensagem8;
    public GameObject mensagem9;
    public GameObject mensagem10;

    private Animator animMensagem7;
    private Animator animMensagem8;
    private Animator animMensagem9;
    private Animator animMensagem10;

    void Start()
    {
        animMensagem7 = mensagem7.GetComponent<Animator>();
        animMensagem8 = mensagem8.GetComponent<Animator>();
        animMensagem9 = mensagem9.GetComponent<Animator>();
        animMensagem10 = mensagem10.GetComponent<Animator>();

        player = GetComponent<Transform>();
        vidaPersonagem = GetComponent<VidaPersonagem>();
        AtivarCapangas("CapangaAndar2", 2); // Ativar capangas do andar 2
    }

    void Update()
    {
        if (boolandar2)
        {
            VerificarCapangasMortos(2);
        }
        else if (boolandar3)
        {
            VerificarCapangasMortos(3);
        }
        else if (boolandar4)
        {
            VerificarCapangasMortos(4);
        }
    }

    // Função que será chamada quando o jogador chegar no próximo andar
    public void AtivarCapangas(string nomeCapanga, int andar)
    {
        List<GameObject> capangasLista = GetCapangasListaPorAndar(andar);
        capangasLista.Clear();

        // Encontra todos os capangas pelo nome específico e adiciona à lista correspondente
        GameObject[] capangasArray = GameObject.FindGameObjectsWithTag("capanga");
        foreach (GameObject capanga in capangasArray)
        {
            if (capanga.activeInHierarchy && capanga.name.Contains(nomeCapanga)) // Verifica se o nome contém o identificador do andar
            {
                capangasLista.Add(capanga); // Adiciona apenas capangas ativos à lista
                Debug.Log($"Capanga {capanga.name} adicionado à lista do andar {andar}.");
            }
        }

        Debug.Log($"Total de capangas no andar {andar}: {capangasLista.Count}");
    }

    private List<GameObject> GetCapangasListaPorAndar(int andar)
    {
        switch (andar)
        {
            case 2:
                return capangasAndar2;
            case 3:
                return capangasAndar3;
            case 4:
                return capangasAndar4;
            default:
                return new List<GameObject>();
        }
    }

    // Verifica se todos os capangas do andar atual estão mortos
    private void VerificarCapangasMortos(int andar)
    {
        List<GameObject> capangasLista = GetCapangasListaPorAndar(andar);

        // Cria uma lista temporária para armazenar capangas que devem ser removidos
        List<GameObject> capangasParaRemover = new List<GameObject>();

        // Verifica todos os capangas na lista
        foreach (GameObject capanga in capangasLista)
        {
            // Pega o script CapangaSegueEMorre de cada capanga
            CapangaSegueEMorre capangaScript = capanga.GetComponent<CapangaSegueEMorre>();

            // Se o capanga estiver morto, adiciona ele à lista de remoção
            if (capangaScript != null && capangaScript.morreu)
            {
                capangasParaRemover.Add(capanga);
            }
        }

        // Remove todos os capangas mortos da lista original
        foreach (GameObject capanga in capangasParaRemover)
        {
            capangasLista.Remove(capanga);
            Debug.Log($"Capanga {capanga.name} removido da lista do andar {andar}.");
        }

        // Verifica se não há mais capangas
        if (capangasLista.Count == 0)
        {
            if (andar == 3 && !tocouCutScene3)
            {
                StartCoroutine(AcaoQuandoCapangasAndar3Morreram());
            }
            else if (andar == 4 && !tocouCutScene4)
            {
                StartCoroutine(AcaoQuandoCapangasAndar4Morreram());
            }
            else
            {
                AcaoQuandoSemCapangas();
            }
        }
    }

    // Função que será chamada quando todos os capangas estiverem mortos
    void AcaoQuandoSemCapangas()
    {
        animatorEscada.SetTrigger("desceu");
        Debug.Log("Todos os capangas estão mortos!");
    }

    public IEnumerator AcaoQuandoCapangasAndar3Morreram()
    {
        Debug.Log("Todos os capangas estão mortos do andar 3!");
        cutsceneSubiuParkour1.Play();
        tocouCutScene3 = true;
        vidaPlayer.SetActive(false);
        Super.SetActive(false);
        somNoti.Play();
        mensagem7.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem7.SetTrigger("fechou");
    }

    public IEnumerator AcaoQuandoCapangasAndar4Morreram()
    {
        Debug.Log("Todos os capangas estão mortos do andar 4!");
        cutsceneSubiuParkour2.Play();
        tocouCutScene4 = true;
        vidaPlayer.SetActive(false);
        Super.SetActive(false);
        somNoti.Play();
        mensagem10.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem10.SetTrigger("fechou");
    }

    public IEnumerator DesativarParkour1()
    {
        cutsceneDesceuParkour1.Play();

        yield return new WaitForSeconds((float)cutsceneDesceuParkour1.duration);
        boolandar4 = true;
        boolandar3 = false;
        vidaPersonagem.vidaAtual = 3;
         vidaPlayer.SetActive(true);
        Super.SetActive(true);
        AtivarCapangas("CapangaAndar4", 4); // Ativar capangas do andar 4
        somNoti.Play();
        mensagem8.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem8.SetTrigger("fechou");
        somNoti.Play();
        mensagem9.SetActive(true);
        yield return new WaitForSeconds(5);
        animMensagem9.SetTrigger("fechou");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("andar2"))
        {
            player.position = andar2.position;
            StartCoroutine(verificarFasePredio.Andar2());
            verificarFasePredio.AtualizarControladorFases();
        }
        if (other.gameObject.CompareTag("andar3"))
        {
            player.position = andar3.position;
            StartCoroutine(verificarFasePredio.Andar3());
            verificarFasePredio.AtualizarControladorFases();
        }
        if (other.gameObject.CompareTag("falsoandar4"))
        {
            StartCoroutine(DesativarParkour1());
        }

        if (other.gameObject.CompareTag("andar4"))
        {
            player.position = andar4.position;
            StartCoroutine(verificarFasePredio.Andar4());
            verificarFasePredio.AtualizarControladorFases();
        }
    }
}
