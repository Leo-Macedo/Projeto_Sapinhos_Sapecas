using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TeletransportePorta : MonoBehaviour
{
    //Teletransporte
    public GameObject player;
    public Transform tp;
    public Transform tpsalazida;

    void Start() { }

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        //Entrar na porta
        if (other.gameObject.CompareTag("porta"))
        {
            EntrarNaPortaEIrParaSala();
        }
        if (other.gameObject.CompareTag("porta2"))
        {
             player.transform.position = new Vector3(
            tpsalazida.transform.position.x,
            tpsalazida.transform.position.y,
            tpsalazida.transform.position.z
        );
        }
    }

    private void EntrarNaPortaEIrParaSala()
    {
        player.transform.position = new Vector3(
            tp.transform.position.x,
            tp.transform.position.y,
            tp.transform.position.z
        );
        Debug.Log("Entrou na porta");
        Invoke("DesativarTxt", 3);
    }
}
