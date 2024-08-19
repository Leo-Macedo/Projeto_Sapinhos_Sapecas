using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetarPlayerPrefs : MonoBehaviour
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs resetado no Editor.");
    }
}
