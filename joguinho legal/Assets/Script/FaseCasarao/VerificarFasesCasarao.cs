using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificarFasesCasarao : MonoBehaviour
{
    public Animator animatorFade;
    public CinemachineFreeLook freeLookCamera;
    public GameObject rivaldo;
    public GameObject romario;
    public GameObject[] canvas;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator PassouFase()
    {
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2);
       
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
        
        rivaldo.SetActive(true);
        freeLookCamera.Follow = rivaldo.transform;
        freeLookCamera.LookAt = rivaldo.transform;
        yield return new WaitForSeconds(1f);
        
        animatorFade.SetTrigger("abrir");
        Debug.Log("funcionouuuuuuuuuuuuuuuuuuuuuuu");
        romario.SetActive(false);



    }
}
