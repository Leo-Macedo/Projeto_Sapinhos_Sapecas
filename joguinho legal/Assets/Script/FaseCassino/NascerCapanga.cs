using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NascerCapanga : MonoBehaviour
{
    //Nascer capangas aleatoriamente a cada 7 segundos
    public Transform[] spawnpoints;

    public GameObject capanga;

    public GameObject vilao;
    public GameObject player;
    private VidaPersonagem vidaPersonagem;
    private VidaVilao vidaVilao;
    public float tempo = 10;

    private void Start()
    {
        vidaVilao = vilao.GetComponent<VidaVilao>();
        vidaPersonagem = player.GetComponent<VidaPersonagem>();
        InvokeRepeating("SpawnCapanga", 1, tempo);
    }
    
    public void SpawnCapanga()
    {
     if(!vidaPersonagem.acabouojogo)  
     {
        Quaternion valorRotacao = Quaternion.Euler(0f, 90f, 0f);
        int r = Random.Range(0, spawnpoints.Length);
        GameObject Capanga = Instantiate(capanga, spawnpoints[r].position, valorRotacao);
        Capanga.tag = "capanga";
     }
     else
     Debug.Log("CHEGA DE NASCER CAPANGA");
    }
}
