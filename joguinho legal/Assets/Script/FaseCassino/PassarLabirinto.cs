using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassarLabirinto : MonoBehaviour
{
    private int contador = 0;
    public Transform[] waypoints;
    private Transform player;
    void Start()
    {
        player = GetComponent<Transform>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("certo"))
        {
            contador++;
            VerificarLabirinto();
        }
        if (other.gameObject.CompareTag("errado"))
        {
            contador = 0;
            VerificarLabirinto();
        }
    }

    private void VerificarLabirinto()
    {
        switch(contador)
        {
            case 0:
                TP(0);
                break;

            case 1:
                TP(1);
                break;

            case 2:
                TP(2);
                break;

            case 3:
                TP(3);
                break;

            case 4:
                TP(4);
                break;
        }
    }

    private void TP(int num)
    {
        player.position = waypoints[num].position;
        player.rotation = waypoints[num].rotation;
    }
}
