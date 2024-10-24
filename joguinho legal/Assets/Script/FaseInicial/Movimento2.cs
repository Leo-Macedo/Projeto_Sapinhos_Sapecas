using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimento2 : MonoBehaviour
{   
    public Animator animatorFade;
    public AudioSource somPassos;
    public AudioClip[] audiosPassos;
    public AudioSource somIdle;
    public AudioClip[] audiosIdle;

    [SerializeField]
    public float velocidade;

    [SerializeField]
    public float veloAndando;

    [SerializeField]
    public float veloCorrendo;
    private Animator anim;

    private float inputX;
    private float inputZ;
    private Vector3 direcao;

    [SerializeField]
    private float forcaPulo = 10f;
    private Rigidbody rb;

    [SerializeField]
    private LayerMask layermask;

    [SerializeField]
    private float groundCheckSize;

    [SerializeField]
    private Vector3 groundCheckPosition;

    public bool isGrounded;

    [SerializeField]
    private CinemachineFreeLook cinemachineCamera;

    [SerializeField]
    private int id;

    private void Awake()
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.CriarBanco();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        velocidade = veloAndando;
        Carregar(id);
    }

    void Update()
    {
        HandleMovement();
        HandleActions();
        HandleSavingAndDeleting();
    }

    private void HandleMovement()
    {
        Andar();
        Correr();
        Pular();
    }

    private void HandleActions()
    {
        Farpar();
    }

    private void HandleSavingAndDeleting()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Salvar(id);
            Debug.Log("Salvou");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Deletar(id);
            Debug.Log("Deletou");
        }
    }

    private void Andar()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        Vector3 forward = cinemachineCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cinemachineCamera.transform.right;
        right.y = 0;
        right.Normalize();

        direcao = (forward * inputZ + right * inputX).normalized;

        if (direcao.magnitude >= 0.1f)
        {
            transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);
            anim.SetBool("andando", true);

            Quaternion toRotation = Quaternion.LookRotation(direcao, Vector3.up);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                toRotation,
                5 * Time.deltaTime
            );
        }
        else
        {
            anim.SetBool("andando", false);
        }
    }

    private void Correr()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && direcao != Vector3.zero)
        {
            velocidade = veloCorrendo;
            anim.SetBool("correndo", true);
            anim.SetBool("andando", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || direcao == Vector3.zero)
        {
            velocidade = veloAndando;
            anim.SetBool("correndo", false);
        }
    }

    public void Pular(float multiplicador = 1f)
    {
        Vector3 escalaAtual = transform.localScale;

        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);
        
        var groundcheck = Physics.OverlapSphere(
            transform.position + posicaoVerificada,
            tamanhoVerificado,
            layermask
        );

        isGrounded = groundcheck.Length != 0;

        anim.SetBool("pulo", !isGrounded);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * forcaPulo * multiplicador, ForceMode.Impulse);
        }
    }

    private void Farpar()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("emote1");
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 escalaAtual = transform.localScale;

        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + posicaoVerificada, tamanhoVerificado);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("voltar"))
        {
            StartCoroutine(Voltar());
        }
    }

    public IEnumerator Voltar()
    {
        veloAndando = 0;
        veloCorrendo = 0;
        animatorFade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("CenaInicial");
    }

    public void SomPassos()
    {
        somPassos.PlayOneShot(audiosPassos[Random.Range(0, audiosPassos.Length)]);
    }

    public void SomIdle()
    {
        somIdle.PlayOneShot(audiosIdle[Random.Range(0, audiosIdle.Length)]);
    }

    private void Salvar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.InserirPosicao(
            id,
            transform.position.x,
            transform.position.y,
            transform.position.z
        );
    }

    private void Deletar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        bancoDeDados.NovoJogo();
    }

    private void Carregar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados();
        var leitura = bancoDeDados.LerPosicao(id);
        if (leitura != null)
        {
            while (leitura.Read())
            {
                transform.position = new Vector3(
                    leitura.GetFloat(1),
                    leitura.GetFloat(2),
                    leitura.GetFloat(3)
                );
            }
        }
        leitura.Close();
        bancoDeDados.FecharConexao();
    }
}
