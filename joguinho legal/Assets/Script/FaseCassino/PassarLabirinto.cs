using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassarLabirinto : MonoBehaviour
{
    private int contador = 0;
    public Transform[] waypoints;
    private Transform player;
    public Animator animatorFade;
    private bool flag;

    void Start()
    {
        player = GetComponent<Transform>();
    }

    void Update() { }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("certo") && !flag)
        {
            contador++;
            Debug.Log("Caminho certo: 0" + contador);

            VerificarLabirinto();
            flag = true;
        }
        if (other.gameObject.CompareTag("errado") && !flag)
        {
            contador = 0;
            Debug.Log("Caminho errado: " + contador);

            VerificarLabirinto();
            flag = true;
        }
    }

    private void VerificarLabirinto()
    {
        switch (contador)
        {
            case 0:
                StartCoroutine(TP(0));
                break;

            case 1:
                StartCoroutine(TP(1));
                break;

            case 2:
                StartCoroutine(TP(2));
                break;

            case 3:
                StartCoroutine(TP(3));
                break;

            case 4:
                StartCoroutine(TP(4));
                break;
            case 5:
                StartCoroutine(TrocarCena());
                break;
        }
    }

    public IEnumerator TP(int num)
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }
        yield return new WaitForSeconds(2);
        player.position = waypoints[num].position;
        player.rotation = Quaternion.Euler(0, waypoints[num].rotation.eulerAngles.y, 0); // Define apenas a rotação no eixo Y
        yield return new WaitForSeconds(0.1f);
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("abrir");
        }
        flag = false;
    }

    public IEnumerator TrocarCena()
    {
        if (animatorFade != null)
        {
            animatorFade.SetTrigger("fechar");
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Cassino");
    }
}
