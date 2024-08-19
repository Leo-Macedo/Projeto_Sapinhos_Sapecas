using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvarFases : MonoBehaviour
{
    public GameObject predioCompletado;
    public GameObject taverna;
    public GameObject cassino;

    

    // Start is called before the first frame update
    void Start()
    {
        VerificarProgresso();
       
    }

    // Update is called once per frame
    public void VerificarProgresso()
    {
        if (PlayerPrefs.GetInt("PredioCompletado", 0) == 1)
        {
            taverna.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Taverna Completada", 0) == 1)
        {
            cassino.SetActive(true);
        }
    }
}
