using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimento2 : MonoBehaviour
{
    public AudioSource somPulo;
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
    private bool correndo = false;

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
    public float rotationSpeed;

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

        for (int i = 0; i <= 20; i++) // Verifica até 20 botões (ajuste se necessário)
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                Debug.Log("Botão pressionado: " + i);
            }
        }
    }

    private void HandleMovement()
    {
        Andar();
        Correr();
        Pular();
        Rotacao();
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

        if (Camera.main != null)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();

            direcao = (forward * inputZ + right * inputX).normalized;

            anim.SetFloat("inputX", inputX);
            anim.SetFloat("inputY", inputZ);

            if (direcao.magnitude >= 0.1f)
            {
                transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);
            }
        }
        else
        {
            Debug.Log("Camera.main não foi encontrada.");
        }
    }

    private void Rotacao()
    {
        // Captura o movimento horizontal do mouse
        float mouseXInput = Input.GetAxis("CameraX");

        // Ignorar pequenas entradas
        if (Mathf.Abs(mouseXInput) > 0.01f)
        {
            transform.Rotate(0f, mouseXInput * rotationSpeed, 0f);
        }
    }

    private void Correr()
    {
        // Verifica se o botão de correr foi pressionado.
        if (Input.GetButtonDown("Correr"))
        {
            // Alterna o estado de corrida.
            correndo = !correndo;

            if (correndo)
            {
                // Se for para correr, define a velocidade para correr e ativa a animação de corrida.
                velocidade = veloCorrendo;
                inputZ = 1f; // Movimento para frente
                anim.SetBool("correndo", true);
                anim.SetBool("andando", false);
            }
            else
            {
                // Se for para parar de correr, define a velocidade para andar e para a animação de corrida.
                velocidade = veloAndando;
                inputZ = 0f; // Para de se mover
                anim.SetBool("correndo", false);
                anim.SetBool("andando", true);
            }
        }

        // Se o personagem estiver parado, para de correr.
        if (inputZ == 0 && Mathf.Abs(inputX) < 0.1f)
        {
            if (correndo)
            {
                correndo = false;
                velocidade = veloAndando;
                anim.SetBool("correndo", false);
                anim.SetBool("andando", true);
            }
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
            GameObject somChefeMorreu = GameObject.FindWithTag("sompulo");
            if (somChefeMorreu != null)
            {
                AudioSource audioSource = somChefeMorreu.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
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
