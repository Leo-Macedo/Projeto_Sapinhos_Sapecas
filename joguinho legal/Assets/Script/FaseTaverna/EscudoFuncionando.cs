using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EscudoFuncionando : MonoBehaviour
{
    public int contadorEscudo;
    public Animator animatoPorta;
    [Header("Referencias")]
    public AudioSource somPegou;
    public TextMeshProUGUI txtEscudosPegos;
    public GameObject escudo;
    public bool escudoAtivo = false;

    void Start() { }

    void Update()
    {
        AtivarEscudo();
        if( contadorEscudo == 3)
        {
            animatoPorta.SetTrigger("abrir");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("escudo"))
        {
            PegarEscudo(other);
        }
    }

    public void PegarEscudo(Collider other)
    {
        Destroy(other.gameObject);
        contadorEscudo += 1;
        somPegou.Play();
        txtEscudosPegos.text = contadorEscudo + "";
    }

    public void AtivarEscudo()
    {
        if (Input.GetKeyDown(KeyCode.Z) && contadorEscudo > 0)
        {
            escudo.SetActive(true);
            escudoAtivo = true;
            contadorEscudo -= 1;
            txtEscudosPegos.text = contadorEscudo + "";
            Invoke("DesativarEscudo", 2);
        }
    }

    public void DesativarEscudo()
    {
        escudo.SetActive(false);
        escudoAtivo = false;
    }

    
}
