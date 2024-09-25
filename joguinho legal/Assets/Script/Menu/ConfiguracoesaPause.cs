using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfiguracoesPause : MonoBehaviour
{
    public AudioMixer aMixer;
    public Slider volumeSlider;

    void Start()
    {
        // Verifica se há um valor salvo e o carrega
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
            volumeSlider.value = savedVolume;
            Debug.Log("Valor carregado do PlayerPrefs: " + savedVolume);
        }
        else
        {
            volumeSlider.value = 4; // Valor padrão
            Debug.Log("Nenhum valor salvo encontrado, atribuindo valor padrão: 1");
        }

        // Aplica o volume com base no valor carregado ou padrão
        ChangeValue();
    }

    public void ChangeValue()
    {
        // Usa o valor do slider no switch
        int sliderValue = Mathf.RoundToInt(volumeSlider.value);

        Debug.Log("Valor do Slider antes de aplicar no AudioMixer: " + sliderValue);

        switch (sliderValue)
        {
            case 0:
                aMixer.SetFloat("Music", -88);
                Debug.Log("Volume ajustado para -88 (Silencioso)");
                break;
            case 1:
                aMixer.SetFloat("Music", -40);
                Debug.Log("Volume ajustado para -40");
                break;
            case 2:
                aMixer.SetFloat("Music", -20);
                Debug.Log("Volume ajustado para -20");
                break;
            case 3:
                aMixer.SetFloat("Music", -10);
                Debug.Log("Volume ajustado para -10");
                break;
            case 4:
                aMixer.SetFloat("Music", 0);
                Debug.Log("Volume ajustado para 0 (Volume normal)");
                break;
            case 5:
                aMixer.SetFloat("Music", 10);
                Debug.Log("Volume ajustado para 10 (Mais alto)");
                break;
        }

        // Salva o valor do slider em PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        PlayerPrefs.Save();

        Debug.Log("Valor do Slider salvo no PlayerPrefs: " + volumeSlider.value);
    }
}
