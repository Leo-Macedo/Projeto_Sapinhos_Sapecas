using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControleSensibilidadeCamera : MonoBehaviour
{
    public Slider sliderSensibilidade;
    public TextMeshProUGUI textoSensibilidade;
    private bool jogoPausado = false;
    public GameObject telapause;
    private List<Movimento2> movimento2List = new List<Movimento2>();
    private List<CameraFollow> camerasFollow = new List<CameraFollow>();
    public bool podePausar = false;

    void Start()
    {
                Time.timeScale = 1;

        // Encontra todos os objetos com a tag "Player", mesmo que desativados
        GameObject[] allPlayers = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allPlayers)
        {
            if (obj.CompareTag("Player"))
            {
                Movimento2 mov2 = obj.GetComponent<Movimento2>();
                if (mov2 != null)
                {
                    movimento2List.Add(mov2);
                }
            }
        }

        // Encontra todas as câmeras com a tag "MainCamera", mesmo que desativadas
        Camera[] allCameras = Resources.FindObjectsOfTypeAll<Camera>();
        foreach (Camera cam in allCameras)
        {
            if (cam.CompareTag("MainCamera"))
            {
                CameraFollow camFollow = cam.GetComponent<CameraFollow>();
                if (camFollow != null)
                {
                    camerasFollow.Add(camFollow);
                }
            }
        }

        float sensibilidadeSalva = PlayerPrefs.GetFloat("Sensibilidade", 5f);
        sliderSensibilidade.value = sensibilidadeSalva;
        AtualizarTextoSensibilidade(sensibilidadeSalva);
        AtualizarSensibilidadeCamera(sensibilidadeSalva);
        sliderSensibilidade.onValueChanged.AddListener(AtualizarSensibilidadeCamera);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && podePausar)
        {
            if (jogoPausado)
            {
                DespausarJogo();
            }
            else
            {
                PausarJogo();
            }
        }
    }

    void PausarJogo()
    {
        Time.timeScale = 0;
        telapause.SetActive(true);
        jogoPausado = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DespausarJogo()
    {
        Time.timeScale = 1;
        telapause.SetActive(false);
        jogoPausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void AtualizarSensibilidadeCamera(float valor)
    {
        foreach (var mov2 in movimento2List)
        {
            mov2.rotationSpeed = valor * 3f;
        }

        foreach (var camFollow in camerasFollow)
        {
            camFollow.rotationSpeed = valor * 0.66f;
        }

        AtualizarTextoSensibilidade(valor);
        PlayerPrefs.SetFloat("Sensibilidade", valor);
        PlayerPrefs.Save();
    }

    void AtualizarTextoSensibilidade(float valor)
    {
        textoSensibilidade.text = "Sensibilidade: " + valor.ToString("F1");
    }
}
