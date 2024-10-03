using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificarTutorial : MonoBehaviour
{
    public Animator animatorPlaca;
    public Animator animatorPortal;
    public CapangaSegueEMorre capangaSegueEMorre;
    public GameObject sliderSuper;
    void Start()
    {
        
    }

    void Update()
    {
        if(capangaSegueEMorre.morreu == true)
        {
            animatorPlaca.SetTrigger("subiu");
            animatorPortal.SetTrigger("subiu");
            sliderSuper.SetActive(true);
        }
    }
}
