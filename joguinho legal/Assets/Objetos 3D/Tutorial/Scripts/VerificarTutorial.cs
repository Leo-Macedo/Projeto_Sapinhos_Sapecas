using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificarTutorial : MonoBehaviour
{
    public ControleSensibilidadeCamera controleSensibilidadeCamera;

    public Animator animatorPlaca;
    public Animator animatorPortal;
    public CapangaSegueEMorre capangaSegueEMorre;
    public GameObject sliderSuper;

    void Start()
    {
        controleSensibilidadeCamera.podePausar = true;
        Time.timeScale = 1f;

    }

    void Update()
    {   
        
        if (capangaSegueEMorre.morreu == true)
        {
            animatorPlaca.SetTrigger("subiu");
            animatorPortal.SetTrigger("subiu");
            sliderSuper.SetActive(false);
        }
    }
}
