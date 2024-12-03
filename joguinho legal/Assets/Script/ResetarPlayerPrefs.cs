using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResetarPlayerPrefs : MonoBehaviour
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs resetado no Editor.");
    }

    [MenuItem("Tools/CompletarPredio")]
    public static void CompletarPredio()
    {
        PlayerPrefs.SetInt("PredioCompletado", 1);
        Debug.Log("PredioCompletado no Editor.");

        SalvarEVerificarProgresso();
    }

    [MenuItem("Tools/CompletarTaverna")]
    public static void CompletarTaverna()
    {
        PlayerPrefs.SetInt("PredioCompletado", 1);
        PlayerPrefs.SetInt("TavernaCompletada", 1);
        Debug.Log("TavernaCompletada no Editor.");

        SalvarEVerificarProgresso();
    }

    [MenuItem("Tools/CompletarCassino")]
    public static void CompletarCassino()
    {
        PlayerPrefs.SetInt("PredioCompletado", 1);
        PlayerPrefs.SetInt("TavernaCompletada", 1);
        PlayerPrefs.SetInt("CassinoCompletado", 1);
        Debug.Log("CassinoCompletado no Editor.");

        SalvarEVerificarProgresso();
    }

    [MenuItem("Tools/VerificarProgresso")]
    public static void VerificarProgresso()
    {
        SalvarFases salvarFases = GameObject.FindFirstObjectByType<SalvarFases>();
        if (salvarFases != null)
        {
            salvarFases.VerificarProgresso();
            Debug.Log("VerificarProgresso foi chamado.");
        }
        else
        {
            Debug.LogWarning("Não foi possível encontrar o script SalvarFases na cena.");
        }
    }

    private static void SalvarEVerificarProgresso()
    {
        SalvarFases salvarFases = GameObject.FindFirstObjectByType<SalvarFases>();
        if (salvarFases != null)
        {
            salvarFases.VerificarProgresso();
            Debug.Log("VerificarProgresso foi chamado.");
        }
        else
        {
            Debug.LogWarning("Não foi possível encontrar o script SalvarFases na cena.");
        }
    }

    [MenuItem("Tools/AtualizarControladorFasesCasarao")]

    public static void AtualizarControladorFasesCasarao()
    {
        PlayerPrefs.SetInt("ControladorFasesCasarao", 3);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = ");
    }


    [MenuItem("Tools/AtualizarControladorFasesPredio")]

    public static void AtualizarControladorFasesPredio()
    {
        PlayerPrefs.SetInt("ControladorFasesPredio", 2);
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas imediatamente
        Debug.Log("PlayerPrefs + 1 = ");
    }
    #endif
}
