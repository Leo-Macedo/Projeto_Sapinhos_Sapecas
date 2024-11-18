using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SenhaCor : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    [Header("som")]
    public AudioSource[] som;
    public PassarFasesCasarao passarFasesCasarao;

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

    [Header("Mensagens")]
    public AudioSource somNoti;
    public GameObject mensagem;
    public Animator animatorMSG;

    void Start()
    {
        // Adiciona um ouvinte de clique a cada botão numérico
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i + 1; // Define o número correspondente ao botão
            numberButtons[i].onClick.AddListener(() => OnNumberButtonClick(number));
        }
    }

    // Adiciona o número clicado à senha inserida e atualiza a exibição
    public void OnNumberButtonClick(int number)
    {
        som[0].Play();
        if (senhainserida.Length < 4) // Verifica se a senha inserida tem menos de 4 dígitos
        {
            senhainserida += number.ToString(); // Adiciona o número à senha inserida
            telasenha.text = senhainserida; // Atualiza o texto da senha

            // Verifica se a senha agora tem 4 dígitos
            if (senhainserida.Length == 4)
            {
                CheckPassword(); // Chama a verificação da senha
            }
        }
    }

    // Verifica se a senha inserida está correta e realiza ações apropriadas
    public void CheckPassword()
    {
        if (senhainserida == senhacorreta) // Verifica se a senha inserida é a correta
        {
            StartCoroutine(CertaResposta());
        }
        else
        {
            som[2].Play();
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
        controleSensibilidadeCamera.podePausar = true;
    }

    // Ativa a mensagem de interagir com a senha e abre o painel quando a tecla F é pressionada
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cliqueaqui.SetActive(true); // Ativa a mensagem de interagir

            if (Input.GetKeyDown(KeyCode.F)) // Verifica se a tecla F foi pressionada
            {
                Cursor.lockState = CursorLockMode.None; // Destrava o cursor
                Cursor.visible = true; // Torna o cursor visível
                painelsenha.SetActive(true); // Ativa o painel da senha
                controleSensibilidadeCamera.podePausar = false;
            }
        }
    }

    // Desativa a mensagem de interagir quando o jogador sai da área de colisão
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            cliqueaqui.SetActive(false); // Desativa a mensagem de interagir
    }

    public IEnumerator CertaResposta()
    {
        som[1].Play();
        txtsenhacorreta.SetActive(true); // Ativa a mensagem de senha correta
        senhainserida = ""; // Limpa a senha inserida
        telasenha.text = senhainserida; // Atualiza o texto da senha
        yield return new WaitForSeconds(2);
        Fechar();
        passarFasesCasarao.completouSenha = true;
        if (somNoti != null && mensagem != null && animatorMSG != null)
        {
            somNoti.Play();
            mensagem.SetActive(true);
            yield return new WaitForSeconds(5);
            animatorMSG.SetTrigger("fechou");
        }
    }
}
