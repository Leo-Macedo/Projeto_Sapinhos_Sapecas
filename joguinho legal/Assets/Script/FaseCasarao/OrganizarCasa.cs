using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizarCasa : MonoBehaviour
{
    public GameObject geladeira;
    public GameObject sofa;
    public GameObject privada;

    public Transform lugarGeladeira;
    public Transform lugarSofa;
    public Transform lugarPrivada;

    public Transform lugarMaoGeladeira;
    public Transform lugarMaoSofa;
    public Transform lugarMaoPrivada;

    public bool pegouGeladeira;
    public bool pegouSofa;
    public bool pegouPrivada;

    public GameObject txtColetar;
    public GameObject txtColocar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("geladeira") && !pegouGeladeira)
        {
            txtColetar.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                pegouGeladeira = true;
                // colocar parente
                geladeira.transform.position = lugarMaoGeladeira.transform.position;
                geladeira.transform.rotation = lugarMaoGeladeira.transform.rotation;
            }

        }

        else if (other.gameObject.CompareTag("sofa") && !pegouSofa)
        {
            txtColetar.SetActive(true);

        }

        else if (other.gameObject.CompareTag("privada") && !pegouPrivada)
        {
            txtColetar.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("geladeira") && !pegouGeladeira)
        {
            txtColetar.SetActive(true);

        }

        else if (other.gameObject.CompareTag("sofa") && !pegouSofa)
        {
            txtColetar.SetActive(true);

        }

        else if (other.gameObject.CompareTag("privada") && !pegouPrivada)
        {
            txtColetar.SetActive(true);

        }
    }
}
