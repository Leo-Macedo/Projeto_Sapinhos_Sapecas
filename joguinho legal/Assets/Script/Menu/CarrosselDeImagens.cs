using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrosselDeImagens : MonoBehaviour
{
    [Header("Configuração do Carrossel")]
        private AudioSource somclick;

    public List<GameObject> imagens; // Lista de imagens do carrossel
    private int indiceAtual = 0; // Índice da imagem atual
    public GameObject controles;

    void Start()
    {
        // Ativa apenas a primeira imagem no início
        AtualizarCarrossel();

        GameObject sireneObj = GameObject.FindWithTag("somclick");
        if (sireneObj != null)
        {
            somclick = sireneObj.GetComponent<AudioSource>();
           
        }
    }

    public void Avancar()
    {
        // Desativa a imagem atual
        imagens[indiceAtual].SetActive(false);

        // Incrementa o índice (volta ao início se passar do último)
        indiceAtual = (indiceAtual + 1) % imagens.Count;

        // Ativa a próxima imagem
        imagens[indiceAtual].SetActive(true);
                somclick.Play();

    }

    public void Retroceder()
    {
        // Desativa a imagem atual
        imagens[indiceAtual].SetActive(false);

        // Decrementa o índice (volta ao final se ficar menor que zero)
        indiceAtual = (indiceAtual - 1 + imagens.Count) % imagens.Count;

        // Ativa a imagem anterior
        imagens[indiceAtual].SetActive(true);
        somclick.Play();
    }

    private void AtualizarCarrossel()
    {
        // Garante que apenas a imagem atual esteja ativa
        for (int i = 0; i < imagens.Count; i++)
        {
            imagens[i].SetActive(i == indiceAtual);
        }
    }

    public void FecharControles()
    {
        controles.SetActive(false);
        somclick.Play();
    }
     public void AbrirrControles()
    {
        controles.SetActive(true);
        somclick.Play();
    }
}

