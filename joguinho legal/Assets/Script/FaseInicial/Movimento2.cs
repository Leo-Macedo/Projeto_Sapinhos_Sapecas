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

    //colocar o id do banco
    [SerializeField] private int id;

    private void Awake()
    {
        BancoDeDados bancoDeDados = new BancoDeDados(); //instanciando o banco
        bancoDeDados.CriarBanco();
    }

    void Start()
    {
        Velocidade = veloAndando;
        Carregar(id); // Carregar a posição ao iniciar
    }

    void Update()
    {
        andar(); 

        correr();       

        pular();

        farpar();

        

        //Salvar
        if (Input.GetKeyDown(KeyCode.L))
        {
            Salvar(id);
            Debug.Log("Salvou");
        }

         //deletar
        if (Input.GetKeyDown(KeyCode.O))
        {
            deletar(id);
            Debug.Log("Deletou");
        }
    }

    public void andar(){
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
    }

    public void correr(){
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
    }

    public void pular(){
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
    }
    public void farpar(){
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

    private void Salvar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.InserirPosicao(id, transform.position.x, transform.position.y, transform.position.z);
    }

    private void deletar(int id){
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.NovoJogo();
    }

    private void Carregar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        var Leitura = bancoDeDados.LerPosicao(id);
        if (Leitura != null)
        {
            while (Leitura.Read())
            {
                transform.position = new Vector3(Leitura.GetFloat(1), Leitura.GetFloat(2), Leitura.GetFloat(3));
            }
        }
        Leitura.Close(); // fechar o IDataReader após a leitura
        bancoDeDados.FecharConexao();
    }
}
