using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaDanoNoVilao : MonoBehaviour
{
    public bool jaColidiu = false; // Flag para verificar se a colisão foi com o jogador
    private GameObject vilao;
    private VidaVilao vidaVilao;

    void Start()
    {
        vilao = GameObject.FindGameObjectWithTag("Vilao");
        vidaVilao = vilao.GetComponent<VidaVilao>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Se a colisão for com o jogador e ainda não tiver colidido antes
        if (other.gameObject.CompareTag("Vilao") && !jaColidiu)
        {
            vidaVilao.ReceberDanoVilao(1);
            jaColidiu = true;
            GameObject prefab = Resources.Load<GameObject>("Bombadedinehirobow");
            Instantiate(prefab, other.transform.position, Quaternion.identity);
            Destroy(gameObject);

            // Para a caixa ao colidir com o jogador
        }
        if (other.gameObject.CompareTag("outroobj"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            jaColidiu = true;
        }
    }
}
