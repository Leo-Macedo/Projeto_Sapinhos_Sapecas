using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioEfeitoSonoros : MonoBehaviour
{
    public AudioMixer aMixer;
    public Slider volumeSlider;

    void Start()
    {
        // Verifica se há um valor salvo e o carrega
        if (PlayerPrefs.HasKey("EfeitoVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("EfeitoVolume");
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
                aMixer.SetFloat("Efeito", -88);
                break;
            case 1:
                aMixer.SetFloat("Efeito", -40);
                break;
            case 2:
                aMixer.SetFloat("Efeito", -20);
                break;
            case 3:
                aMixer.SetFloat("Efeito", -10);
                break;
            case 4:
                aMixer.SetFloat("Efeito", 0);
                break;
            case 5:
                aMixer.SetFloat("Efeito", 10);
                break;
        }

        // Salva o valor do slider em PlayerPrefs
        PlayerPrefs.SetFloat("EfeitoVolume", volumeSlider.value);
        PlayerPrefs.Save();

    }
}
