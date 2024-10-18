using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class PassarFasesCasarao : MonoBehaviour
{
    public VerificarFasesCasarao VerificarFasesCasarao;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("portaporao"))
        {
            StartCoroutine(VerificarFasesCasarao.PassouFase());
        }
    }

   
}
