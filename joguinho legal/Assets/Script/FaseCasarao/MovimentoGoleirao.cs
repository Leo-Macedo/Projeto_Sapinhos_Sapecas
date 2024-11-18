using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoGoleirao : MonoBehaviour
{
    public Transform pontoInicial;
    public Transform pontoFinal;
    public float velocidade = 2f;

    private float t = 0f; 
    void Update()
    {
        t += Time.deltaTime * velocidade;

        transform.position = Vector3.Lerp(
            pontoInicial.position,
            pontoFinal.position,
            Mathf.PingPong(t, 1)
        );

    }

    public void AumentarVelocidade()
    {
        velocidade += 0.5f;
    }
}
