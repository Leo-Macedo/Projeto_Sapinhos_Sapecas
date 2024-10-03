using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassarTutorial : MonoBehaviour
{
    public GameObject sliderSuper;

    void Start() { }

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("super"))
        {
            sliderSuper.SetActive(true);
        }
        if (other.gameObject.CompareTag("completo"))
        {
            PlayerPrefs.SetInt("TutorialCompletado", 1);
        }
    }
}
