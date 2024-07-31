using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TeletransportePorta : MonoBehaviour
{
    //contagem de capangas mortos
    public int capangasmortos = 0;
    public TextMeshProUGUI txtcapangasmortos;
    public GameObject txtcapangasmortos1;


    //Teletransporte
    public GameObject player;
    public Transform tp;
    public Transform tpp;
    public GameObject portalvoltar;


    //text
    public GameObject txtmate10capanga;

    //Script NascerCapanga
    public GameObject NascerCapanga;
    private NascerCapangaFase2 nascerCapangaFase2;

    //Referencia cutscene
    public PlayableDirector cutscene;
    private bool tocoucutscene;
    void Start()
    {
        nascerCapangaFase2 = NascerCapanga.GetComponent<NascerCapangaFase2>();

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Entrar na porta
        if (other.gameObject.CompareTag("porta"))
        {
            txtcapangasmortos1.SetActive(true);
            player.transform.position = new Vector3(tp.transform.position.x, tp.transform.position.y, tp.transform.position.z);
            Debug.Log("Entrou na porta");
            NascerCapanga.SetActive(true);
            nascerCapangaFase2.ComecarNascer();
            txtmate10capanga.SetActive(true);
            Invoke("DesativarTxt", 3);

        }

    }

    //desativa txt
    public void DesativarTxt()
    {
        txtmate10capanga.SetActive(false);

    }

    //contar capangas mortos
    public void ContarCapngasMortos()
    {
        capangasmortos++;
        txtcapangasmortos.text = "Capangas mortos: " + capangasmortos + "/10";
        if (capangasmortos == 10)
        {
            player.transform.position = new Vector3(tpp.transform.position.x,
            tpp.transform.position.y, tpp.transform.position.z);
            PlayCutscene();
            Debug.Log("PASSOU DE FASE");
            portalvoltar.SetActive(true);

        }
    }

    void PlayCutscene()
    {
        if (tocoucutscene)
            return;

        tocoucutscene = true;
        cutscene.Play();


    }


}
