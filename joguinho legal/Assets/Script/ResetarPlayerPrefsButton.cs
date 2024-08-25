using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetarPlayerPrefsButton : MonoBehaviour
{
    void Start() { }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("CenaInicial");
        Debug.Log("PlayerPrefs resetado no Build.");
    }
}
