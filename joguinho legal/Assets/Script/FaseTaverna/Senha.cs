using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Senha : MonoBehaviour
{
    //Senha
    public TextMeshProUGUI telasenha;
    public string senhacorreta = "1234";
    private string senhainserida = "";

    //Botões
    public Button[] numberButtons; // Array de botões numéricos

    //Objetos para ativar 
    public GameObject txtsenhacorreta;
    public GameObject txtsenhaincorreta;
    public GameObject painelsenha;
    public GameObject cliqueaqui;
    public GameObject porta;



    void Start()
    {
        //Pega o indice do array para sabe que numero foi clicado
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i + 1;
            numberButtons[i].onClick.AddListener(() => OnNumberButtonClick(number));
        }
    }

    void Update()
    {

    }

    //Adiciona o número clicado na senha e mostra na tela
    public void OnNumberButtonClick(int number)
    {
        if (senhainserida.Length < 4)
        {
            senhainserida += number.ToString();
            telasenha.text = senhainserida;
        }
    }

    //Verifica se a senha esta correta e limpa a tela ao clicar enter
    public void CheckPassword()
    {
        if (senhainserida == senhacorreta)
        {
            Debug.Log("Senha correta!");
            txtsenhacorreta.SetActive(true);
            senhainserida = "";
            telasenha.text = senhainserida;
            porta.SetActive(true);

        }
        else
        {
            Debug.Log("Senha incorreta!");
            senhainserida = "";
            telasenha.text = senhainserida;
            txtsenhaincorreta.SetActive(true);
            Invoke("TxtSenhaDesativar", 2);
        }
    }
    //Desativa a msg de senha incorreta
    public void TxtSenhaDesativar()
    {
        txtsenhaincorreta.SetActive(false);

    }

    //Fecha o painel de senha
    public void Fechar()
    {
        painelsenha.SetActive(false);
    }
    //Verifica se o jogador está dentro da área de colisão para ativar a msg de interagir
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            cliqueaqui.SetActive(true);
        //Interagir com a Senha
        if (Input.GetKeyDown(KeyCode.F))
            painelsenha.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            cliqueaqui.SetActive(false);

    }



}
