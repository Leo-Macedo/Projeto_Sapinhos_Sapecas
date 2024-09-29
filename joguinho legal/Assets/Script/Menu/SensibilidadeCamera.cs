using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class ControleSensibilidadeCamera : MonoBehaviour
{
    public CinemachineFreeLook cameraFreeLook;
    public Slider sliderSensibilidade;
    public TextMeshProUGUI textoSensibilidade;
    private bool jogoPausado = false;
    public GameObject telapause;

    void Update()
    {
       // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Alterna entre pausar e despausar
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
        cameraFreeLook.enabled = false;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    public void DespausarJogo()
    {
        Time.timeScale = 1;
        telapause.SetActive(false);
        jogoPausado = false;
        cameraFreeLook.enabled = true; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void Start()
    {
        float sensibilidadeSalva = PlayerPrefs.GetFloat("Sensibilidade", 5f);
        sliderSensibilidade.value = sensibilidadeSalva;
        AtualizarTextoSensibilidade(sensibilidadeSalva);
        AtualizarSensibilidadeCamera(sensibilidadeSalva);
        sliderSensibilidade.onValueChanged.AddListener(AtualizarSensibilidadeCamera);
    }

    void AtualizarSensibilidadeCamera(float valor)
    {
        cameraFreeLook.m_XAxis.m_MaxSpeed = valor * 60f;
        cameraFreeLook.m_YAxis.m_MaxSpeed = valor * 0.4f;
        AtualizarTextoSensibilidade(valor);
        PlayerPrefs.SetFloat("Sensibilidade", valor);
        PlayerPrefs.Save();
    }

    void AtualizarTextoSensibilidade(float valor)
    {
        textoSensibilidade.text = "Sensibilidade: " + valor.ToString("F1");
    }
}
