using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogosNPC : MonoBehaviour
{
    [Header("Diálogo")]
    public string[] dialogueNpc; // Array de strings contendo o diálogo do NPC
    public int dialogueIndex; // Índice atual do diálogo
    public GameObject seta;
    public GameObject txtFParaInteragir;

    [Header("Interface de Diálogo")]
    public GameObject dialoguePanel; // Painel onde o diálogo é exibido
    public Text dialogueText; // Texto do diálogo
    public Text nameNpc; // Texto que exibe o nome do NPC
    public Image imageNpc; // Imagem do NPC
    public Sprite spriteNpc; // Sprite do NPC
    public Animator animator;

    [Header("Controle de Diálogo")]
    public bool readyToSpeak; // Verifica se o jogador está perto o suficiente para falar com o NPC
    public bool startDialogue; // Verifica se o diálogo foi iniciado

    public float segundosletras; // Tempo entre cada letra exibida
    public string nameNpcString; // Nome do NPC a ser exibido

    void Start() { }

    void Update()
    {
        // Verifica se a tecla F é pressionada e se o NPC está pronto para falar
        if (Input.GetKeyDown(KeyCode.F) && readyToSpeak)
        {
            if (!startDialogue)
            {
                StartDialogue(); // Inicia o diálogo se ainda não tiver iniciado
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue(); // Avança para o próximo diálogo se o texto atual for exibido completamente
            }
        }
    }

    void NextDialogue()
    {
        dialogueIndex++; // Avança para o próximo índice de diálogo
        if (dialogueIndex < dialogueNpc.Length)
        {
            StartCoroutine(ShowDialogue()); // Mostra o próximo diálogo
        }
        else
        {
            // Fecha o painel de diálogo e reseta o estado
            animator.SetTrigger("fechou");
            startDialogue = true;
            dialogueIndex = 0;
        }
    }

    public void StartDialogue()
    {
        seta.SetActive(false);
        txtFParaInteragir.SetActive(false);

        nameNpc.text = nameNpcString; // Define o nome do NPC
        imageNpc.sprite = spriteNpc; // Define a imagem do NPC
        startDialogue = true; // Marca o diálogo como iniciado
        dialogueIndex = 0; // Reseta o índice do diálogo
        dialoguePanel.SetActive(true); // Ativa o painel de diálogo
        StartCoroutine(ShowDialogue()); // Inicia a exibição do diálogo
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = ""; // Limpa o texto do diálogo

        // Exibe o texto do diálogo letra por letra
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text += letter; // Adiciona uma letra ao texto
            yield return new WaitForSeconds(segundosletras); // Aguarda um intervalo de tempo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readyToSpeak = true; // Permite que o jogador fale com o NPC

            if (!startDialogue)
            {
                txtFParaInteragir.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readyToSpeak = false; // Impede que o jogador fale com o NPC

            if (!startDialogue)
            {
                txtFParaInteragir.SetActive(false);
            }
        }
    }
}
