using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotaciona : MonoBehaviour
{
    public Transform mainCamera;

    void Start()
    {
        // Se não foi atribuída uma câmera, use a câmera principal
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // Faz o canvas rotacionar para sempre olhar para a câmera
        if (mainCamera != null)
        {
            // Calcule a rotação necessária para que o Canvas do mundo fique voltado para a câmera
        Vector3 lookDirection = mainCamera.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Aplique a rotação ao Canvas
        transform.rotation = rotation;
        }
    }
}
