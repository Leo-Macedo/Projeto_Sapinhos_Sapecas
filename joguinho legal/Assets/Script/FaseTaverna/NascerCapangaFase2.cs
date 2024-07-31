using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NascerCapangaFase2  : MonoBehaviour
{
    //Nascer capangas aleatoriamente a cada 7 segundos
    public Transform[] spawnpoints;

    public GameObject capanga;

    public GameObject vilao;
    public GameObject player;
    //private VidaPersonagem vidaPersonagem;
    //private VidaVilao vidaVilao;
    private void Start()
    {
        //vidaVilao = vilao.GetComponent<VidaVilao>();
        //vidaPersonagem = player.GetComponent<VidaPersonagem>();
    }
    public void ComecarNascer()
    {
        InvokeRepeating("SpawnCapanga", 1, 7);

    }
    public void SpawnCapanga()
    {
    
        Quaternion valorRotacao = Quaternion.Euler(0f, 90f, 0f);
        int r = Random.Range(0, spawnpoints.Length);
        GameObject Capanga = Instantiate(capanga, spawnpoints[r].position, valorRotacao);
        Capanga.tag = "capangamelo";
    }
}
