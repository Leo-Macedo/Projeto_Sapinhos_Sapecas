using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstanciadorDeBolas : MonoBehaviour
{
    public GameObject bolaPrefab; 
    public Transform spawnPoint; 
    public bool podeInstanciar = true;
    public TextMeshProUGUI textGols; 
    public int contadorGols = 0; 
    public GameObject porta;
    public Animator animatorPorta;
    private bool acabou;

    void Start()
    {
        textGols.text = contadorGols + "/10";
    }

    void Update()
    {
        if (podeInstanciar && !acabou)
        {
            Instantiate(bolaPrefab, spawnPoint.position, spawnPoint.rotation);
            podeInstanciar = false;
        }
    }

    public void SomarGols()
    {
        contadorGols++;
        textGols.text = contadorGols + "/10";
        if(contadorGols == 10)
        {
            TerminouGols();
        }
    }

    public void TerminouGols()
    {
        
        porta.SetActive(true); // Ativa a porta
        animatorPorta.SetTrigger("abrir");
        acabou = true;
    
    }
}
