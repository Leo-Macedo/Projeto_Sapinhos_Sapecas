using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DialogoFinal : MonoBehaviour
{
    [Header("Diálogo")]
    public List<string> npcNames; // Lista de nomes dos NPCs
    public List<Sprite> npcSprites; // Lista de sprites dos NPCs
    public List<string> dialogueLines; // Lista de falas, alternando entre os NPCs
    public List<int> npcIndexes; // Índices dos NPCs para cada fala
    public int dialogueIndex; // Índice atual do diálogo
   
    [Header("Interface de Diálogo")]
    public GameObject dialoguePanel; // Painel onde o diálogo é exibido
    public Text dialogueText; // Texto do diálogo
    public Text nameNpc; // Texto que exibe o nome do NPC
    public Image imageNpc; // Imagem do NPC
    public Animator animator;

    [Header("Controle de Diálogo")]
    public bool readyToSpeak; // Verifica se o jogador está perto o suficiente para falar com o NPC
    public bool startDialogue; // Verifica se o diálogo foi iniciado
    public float segundosletras = 0.05f; // Tempo entre cada letra exibida

    void Update()
    {
        // Diálogo começa manualmente ou pode ser disparado via tecla 'F' se necessário.
    }

    void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < dialogueLines.Count)
        {
            StartCoroutine(ShowDialogue());
        }
        else
        {
            CloseDialoguePanel();
        }
    }

    public void StartDialogue()
    {
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        int currentNpc = npcIndexes[dialogueIndex]; // Determina o NPC responsável pela fala atual
        nameNpc.text = npcNames[currentNpc]; // Define o nome do NPC atual
        imageNpc.sprite = npcSprites[currentNpc]; // Define a imagem do NPC atual

        dialogueText.text = "";

        foreach (char letter in dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(segundosletras);
        }

        // Aguarda 3 segundos antes de trocar para a próxima fala ou fechar o painel
        yield return new WaitForSeconds(3f);
        NextDialogue();
    }

    void CloseDialoguePanel()
    {
        dialoguePanel.SetActive(false); // Desativa o painel de diálogo
        startDialogue = false; // Marca o diálogo como encerrado
        dialogueIndex = 0; // Reseta o índice do diálogo
    }

   
}
