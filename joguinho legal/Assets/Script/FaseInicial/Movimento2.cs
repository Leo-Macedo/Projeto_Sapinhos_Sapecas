using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine; // Certifique-se de que o Cinemachine está no topo

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

    //Pular e verificar colisão com chão
    public float forcaPulo = 10f;
    public Rigidbody rb;
    public LayerMask Layermask;
    public bool IsGrounded;
    public float GroundCheckSize;
    public Vector3 GroundCheckPosition;

    // Referência para a câmera
    public CinemachineFreeLook cinemachineCamera;

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

        //Deletar
        if (Input.GetKeyDown(KeyCode.O))
        {
            deletar(id);
            Debug.Log("Deletou");
        }
    }

    public void andar()
    {
        //Movimentar Personagem
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        Vector3 forward = cinemachineCamera.transform.forward;
        forward.y = 0; // Mantenha o movimento no plano horizontal
        forward.Normalize();

        Vector3 right = cinemachineCamera.transform.right;
        right.y = 0;
        right.Normalize();

        Direcao = (forward * InputZ + right * InputX).normalized;

        if (Direcao.magnitude >= 0.1f)
        {
            transform.Translate(Direcao * Velocidade * Time.deltaTime, Space.World);
            anim.SetBool("andando", true);

            Quaternion toRotation = Quaternion.LookRotation(Direcao, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5 * Time.deltaTime);
        }
        else
        {
            anim.SetBool("andando", false);
        }
    }

    public void correr()
    {
        //Correr
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

    public void pular()
    {
        //Pular e verificar se está no chão
        var groundcheck = Physics.OverlapSphere(transform.position + GroundCheckPosition, GroundCheckSize, Layermask);
        IsGrounded = groundcheck.Length != 0;

        anim.SetBool("pulo", !IsGrounded);
        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * forcaPulo, ForceMode.Impulse);
        }
    }

    public void farpar()
    {
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

    private void deletar(int id)
    {
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
