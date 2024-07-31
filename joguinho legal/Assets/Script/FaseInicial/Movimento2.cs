using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Movimento2 : MonoBehaviour
{
    //Movimentar Personagem
    public float Velocidade;
    public float veloAndando;
    public float veloCorrendo;
    public Animator anim;

    float InputX;
    float InputZ;
    Vector3 Direcao;

    //Puar e verificar colisão com chão
    public float forcaPulo = 10f;
    public Rigidbody rb;
    public LayerMask Layermask;
    public bool IsGrounded;
    public float GroundCheckSize;
    public Vector3 GroundCheckPosition;

    void Start()
    {

        Velocidade = veloAndando;
    }

    void Update()
    {
        //Movimentar Personagem
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        Direcao = new Vector3(InputX, 0, InputZ);
        if (InputX != 0 || InputZ != 0)
        {
            transform.Translate(0, 0, Velocidade * Time.deltaTime);
            anim.SetBool("andando", true);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direcao), 5 * Time.deltaTime);
        }
        else
        {
            anim.SetBool("andando", false);
        }

        //correr
        if (Input.GetKeyDown(KeyCode.LeftShift) && Direcao != Vector3.zero)
        {
           
                Velocidade = veloCorrendo;
                anim.SetBool("correndo", true);
                anim.SetBool("andando", false);
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Direcao == Vector3.zero)
            {
                Velocidade = veloAndando;
                anim.SetBool("correndo", false);

            }

        //Pular e verificar se está no chão
        var groundcheck = Physics.OverlapSphere(transform.position + GroundCheckPosition, GroundCheckSize, Layermask);
        if (groundcheck.Length != 0)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        anim.SetBool("pulo", !IsGrounded);
        if (IsGrounded == true && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * forcaPulo, ForceMode.Impulse);
        }

        //Farpar Inimigo
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("emote1");
        }

    }

    //Desenhar bola de colisão
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + GroundCheckPosition, GroundCheckSize);
    }

    //Voltar par vila do brejo
     private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("voltar"))
            SceneManager.LoadScene("CenaInicial");
    
    }

}
