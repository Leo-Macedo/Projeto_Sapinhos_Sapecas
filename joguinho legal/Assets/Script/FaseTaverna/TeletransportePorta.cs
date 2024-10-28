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
    public VerificarFasesTaverna verificarFasesTaverna;

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
        verificarFasesTaverna.AtualizarControladorFases();
        StartCoroutine(verificarFasesTaverna.Porao());
    }

    private void TeletransportarParaSalaZida()
    {
        // Teletransporta o jogador para a sala de trás
        verificarFasesTaverna.AtualizarControladorFases();
        StartCoroutine(verificarFasesTaverna.SalaZida());
        player.transform.position = tpsalazida.position; // Atualiza a posição do jogador
    }
}
