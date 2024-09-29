using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMusica : MonoBehaviour
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
        }
        else
        {
            volumeSlider.value = 4; // Valor padrão
        }

        // Aplica o volume com base no valor carregado ou padrão
        ChangeValue();
    }

    public void ChangeValue()
    {
        // Usa o valor do slider no switch
        int sliderValue = Mathf.RoundToInt(volumeSlider.value);


        switch (sliderValue)
        {
            case 0:
                aMixer.SetFloat("Music", -88);
                break;
            case 1:
                aMixer.SetFloat("Music", -40);
                break;
            case 2:
                aMixer.SetFloat("Music", -20);
                break;
            case 3:
                aMixer.SetFloat("Music", -10);
                break;
            case 4:
                aMixer.SetFloat("Music", 0);
                break;
            case 5:
                aMixer.SetFloat("Music", 10);
                break;
        }

        // Salva o valor do slider em PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        PlayerPrefs.Save();

    }
}
