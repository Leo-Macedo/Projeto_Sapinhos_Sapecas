using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FicarInvisivel : MonoBehaviour
{
    public bool isInvisible = false;
    public float invisibilityDuration = 5f; // Duração da invisibilidade em segundos
    public GameObject objetoInvisivel; // O objeto a ser ativado e desativado
    public GameObject objetoColorido; // O objeto colorido a ser desativado

    private void Update()
    {
        // Ativa a invisibilidade por um período específico (exemplo)
        if (Input.GetKeyDown(KeyCode.I)) // Por exemplo, pressione 'I' para tornar o jogador invisível
        {
            StartCoroutine(AtivarInvisibilidade());
        }
    }

    private IEnumerator AtivarInvisibilidade()
    {
        isInvisible = true;
        SetVisibility(isInvisible); // Torna o objeto invisível
        yield return new WaitForSeconds(invisibilityDuration);
        isInvisible = false;
        SetVisibility(isInvisible); // Torna o objeto visível novamente
    }

    private void SetVisibility(bool invisible)
    {
        if (objetoInvisivel != null)
        {
            objetoInvisivel.SetActive(invisible);
        }

        if (objetoColorido != null)
        {
            objetoColorido.SetActive(!invisible);
        }
    }
}
