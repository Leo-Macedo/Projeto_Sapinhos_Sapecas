using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VerificarFasesCasarao : MonoBehaviour
{
    public Animator animatorFade;
    public CinemachineFreeLook freeLookCamera;
    public GameObject rivaldo;
    public GameObject romario;
    public GameObject ronaldo;
    public GameObject[] canvas;
    public Transform waiPoint;

    void Start() { }

    void Update() { }

    public IEnumerator PassouFase()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);

        if (canvas != null && canvas.Length > 1)
        {
            if (canvas[0] != null) canvas[0].SetActive(false);
            if (canvas[1] != null) canvas[1].SetActive(true);
        }

        if (rivaldo != null)
        {
            rivaldo.SetActive(true);
        }

        if (freeLookCamera != null && rivaldo != null)
        {
            freeLookCamera.Follow = rivaldo.transform;
            freeLookCamera.LookAt = rivaldo.transform;
        }

        yield return new WaitForSeconds(1f);

        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }

        Debug.Log("funcionouuuuuuuuuuuuuuuuuuuuuuu");

        if (romario != null)
        {
            romario.SetActive(false);
        }
    }

    public IEnumerator PassouFasePorao()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);

        if (ronaldo != null && waiPoint != null)
        {
            ronaldo.transform.position = waiPoint.position;
            ronaldo.transform.rotation = waiPoint.rotation;
        }

        yield return new WaitForSeconds(0.1f);

        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }
    }
     public IEnumerator TrocarCena()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Casarao2"); 

    } 
}
