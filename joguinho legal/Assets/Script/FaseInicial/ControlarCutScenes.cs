using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarCutScenes : MonoBehaviour
{
   public ParticleSystem explosao;

   public void Explosao()
   {
        explosao.Play();
        Debug.Log("Explosao");
   }
}
