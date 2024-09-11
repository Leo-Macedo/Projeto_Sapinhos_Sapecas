using System.Collections.Generic;
using UnityEngine;

public class PassarFases : MonoBehaviour
{
    private List<GameObject> capangas = new List<GameObject>(); // Lista para armazenar os capangas
    private Transform player;
    public Transform andar2;
    public Transform andar3;
    public Transform andar4;
    public Animator animatorEscada;
    public VerificarFasePredio verificarFasePredio;

    void Start()
    {
        player = GetComponent<Transform>();

        // Encontra todos os capangas na cena e adiciona à lista
        GameObject[] capangasArray = GameObject.FindGameObjectsWithTag("capanga");
        foreach (GameObject capanga in capangasArray)
        {
            if (capanga.activeInHierarchy) // Verifica se o capanga está ativo
            {
                capangas.Add(capanga); // Adiciona apenas capangas ativos à lista
            }
        }
    }

    void Update()
    {
        // Verifica todos os capangas na lista
        for (int i = capangas.Count - 1; i >= 0; i--)
        {
            // Pega o script CapangaSegueEMorre de cada capanga
            CapangaSegueEMorre capangaScript = capangas[i].GetComponent<CapangaSegueEMorre>();

            // Se o capanga estiver morto, remove ele da lista
            if (capangaScript != null && capangaScript.morreu)
            {
                capangas.RemoveAt(i); // Remove o capanga da lista
            }
        }

        // Verifica se não há mais capangas
        if (capangas.Count == 0)
        {
            AcaoQuandoSemCapangas();
        }
    }

    // Função que será chamada quando todos os capangas estiverem mortos
    void AcaoQuandoSemCapangas()
    {
        animatorEscada.SetTrigger("desceu");
        Debug.Log("Todos os capangas estão mortos!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("andar2"))
        {
            player.position = andar2.position;
            verificarFasePredio.AtualizarControladorFases();
        }
        if (other.gameObject.CompareTag("andar3"))
        {
            player.position = andar3.position;
            verificarFasePredio.AtualizarControladorFases();
        }

        if (other.gameObject.CompareTag("andar4"))
        {
            player.position = andar4.position;
            verificarFasePredio.AtualizarControladorFases();
        }
    }
}
