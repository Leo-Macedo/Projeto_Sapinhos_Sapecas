using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TeletransportePorta : MonoBehaviour
{
    [Header("Teletransporte")]
    public GameObject player; // Referência ao jogador
    public Transform tp; // Ponto de teletransporte para a sala principal
    public Transform tpsalazida; // Ponto de teletransporte para a sala de trás

    void Start() { }

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto colidido é a porta para a sala principal
        if (other.gameObject.CompareTag("porta"))
        {
            EntrarNaPortaEIrParaSala(); // Teletransporta o jogador para a sala principal
        }
        // Verifica se o objeto colidido é a porta para a sala de trás
        else if (other.gameObject.CompareTag("porta2"))
        {
            TeletransportarParaSalaZida(); // Teletransporta o jogador para a sala de trás
        }
    }

    private void EntrarNaPortaEIrParaSala()
    {
        // Teletransporta o jogador para a sala principal
        player.transform.position = tp.position; // Atualiza a posição do jogador
        Debug.Log("Entrou na porta"); // Mensagem no console
        Invoke("DesativarTxt", 3); // Desativa uma mensagem após 3 segundos
    }

    private void TeletransportarParaSalaZida()
    {
        // Teletransporta o jogador para a sala de trás
        player.transform.position = tpsalazida.position; // Atualiza a posição do jogador
    }

    private void DesativarTxt()
    {
        // Implementar lógica para desativar a mensagem se necessário
        // Exemplo: texto de "Entrou na porta" ou algo relacionado
    }
}
