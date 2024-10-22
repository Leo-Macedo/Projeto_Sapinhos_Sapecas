using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaNoGol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("gol"))
        {
            Debug.Log("GOOOOOLLLLL");
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("naogol"))
        {
            Debug.Log("pra fora");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("trave"))
        {
            Debug.Log("na trave");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("goleiro"))
        {
            Debug.Log("defendeu goleiro");
            Destroy(gameObject);
        }
    }
}
