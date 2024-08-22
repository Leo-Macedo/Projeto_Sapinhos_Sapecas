using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SalvarFases : MonoBehaviour
{
    public PlayableDirector cutsceneTaverna1;
    private bool tocouCutSceneTaverna = false;
    public GameObject Taverna;
    
    

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
            if (PlayerPrefs.GetInt("TavernaCompletada", 0) == 0)
            {
            cutsceneTaverna1.Play();
            tocouCutSceneTaverna = true;
            }
        }

        if (PlayerPrefs.GetInt("Taverna Completada", 0) == 1)
        {
           
        }
    }
}
