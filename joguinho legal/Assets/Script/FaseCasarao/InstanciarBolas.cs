using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstanciadorDeBolas : MonoBehaviour
{
    public GameObject cubo;
    public MovimentoGoleirao movimentoGoleirao;
    public GameObject bolaPrefab;
    public Transform spawnPoint;
    public bool podeInstanciar = true;
    public TextMeshProUGUI textGols;
    public int contadorGols = 0;
    public GameObject porta;
    public Animator animatorEscada;
    private bool acabou;
    public GameObject[] debug;

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
        movimentoGoleirao.AumentarVelocidade();

        if (contadorGols == 10)
        {
            TerminouGols();
        }
    }

    public void TerminouGols()
    {
        cubo.SetActive(true);
        textGols.text = " ";
        porta.SetActive(true); // Ativa a porta
        animatorEscada.SetTrigger("descer");
        acabou = true;
    }

    public IEnumerator DebugGol(int tipo)
    {
        debug[tipo].SetActive(true);

        yield return new WaitForSeconds(3.5f);

        debug[tipo].SetActive(false);
        podeInstanciar = true;
        Debug.Log(podeInstanciar);
    }
}
