using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Senha : MonoBehaviour
{
    [Header("Senha")]
    public TextMeshProUGUI telasenha; // Texto que mostra a senha inserida
    public string senhacorreta = "1234"; // Senha correta
    private string senhainserida = ""; // Senha inserida pelo jogador

    [Header("Botões")]
    public Button[] numberButtons; // Array de botões numéricos

    [Header("Objetos para Ativar")]
    public GameObject txtsenhacorreta; // Mensagem de senha correta
    public GameObject txtsenhaincorreta; // Mensagem de senha incorreta
    public GameObject painelsenha; // Painel da senha
    public GameObject cliqueaqui; // Mensagem para interagir com a senha
    public GameObject porta; // Porta a ser desbloqueada

    void Start()
    {
        // Adiciona um ouvinte de clique a cada botão numérico
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i + 1; // Define o número correspondente ao botão
            numberButtons[i].onClick.AddListener(() => OnNumberButtonClick(number));
        }
    }

    void Update() { }

    // Adiciona o número clicado à senha inserida e atualiza a exibição
    public void OnNumberButtonClick(int number)
    {
        if (senhainserida.Length < 4) // Verifica se a senha inserida tem menos de 4 dígitos
        {
            senhainserida += number.ToString(); // Adiciona o número à senha inserida
            telasenha.text = senhainserida; // Atualiza o texto da senha
        }
    }

    // Verifica se a senha inserida está correta e realiza ações apropriadas
    public void CheckPassword()
    {
        if (senhainserida == senhacorreta) // Verifica se a senha inserida é a correta
        {
            Debug.Log("Senha correta!"); // Mensagem de sucesso no console
            txtsenhacorreta.SetActive(true); // Ativa a mensagem de senha correta
            senhainserida = ""; // Limpa a senha inserida
            telasenha.text = senhainserida; // Atualiza o texto da senha
            porta.SetActive(true); // Ativa a porta
        }
        else
        {
            Debug.Log("Senha incorreta!"); // Mensagem de erro no console
            senhainserida = ""; // Limpa a senha inserida
            telasenha.text = senhainserida; // Atualiza o texto da senha
            txtsenhaincorreta.SetActive(true); // Ativa a mensagem de senha incorreta
            Invoke("TxtSenhaDesativar", 2); // Desativa a mensagem de senha incorreta após 2 segundos
        }
    }

    // Desativa a mensagem de senha incorreta
    public void TxtSenhaDesativar()
    {
        txtsenhaincorreta.SetActive(false);
    }

    // Fecha o painel da senha e altera o estado do cursor
    public void Fechar()
    {
        painelsenha.SetActive(false); // Desativa o painel da senha
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor
        Cursor.visible = false; // Torna o cursor invisível
    }

    // Ativa a mensagem de interagir com a senha e abre o painel quando a tecla F é pressionada
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            cliqueaqui.SetActive(true); // Ativa a mensagem de interagir

        if (Input.GetKeyDown(KeyCode.F)) // Verifica se a tecla F foi pressionada
        {
            Cursor.lockState = CursorLockMode.None; // Destrava o cursor
            Cursor.visible = true; // Torna o cursor visível
            painelsenha.SetActive(true); // Ativa o painel da senha
        }
    }

    // Desativa a mensagem de interagir quando o jogador sai da área de colisão
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            cliqueaqui.SetActive(false); // Desativa a mensagem de interagir
    }
}
