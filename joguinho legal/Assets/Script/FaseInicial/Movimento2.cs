using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimento2 : MonoBehaviour
{
    private bool cursorBloqueado = true;

    // Movimentar Personagem
    [SerializeField]
    public float velocidade; // Velocidade atual do personagem

    [SerializeField]
    public float veloAndando; // Velocidade ao andar

    [SerializeField]
    public float veloCorrendo; // Velocidade ao correr
    private Animator anim; // Referência ao Animator para controle de animações

    private float inputX; // Entrada do eixo horizontal (A/D ou setas esquerda/direita)
    private float inputZ; // Entrada do eixo vertical (W/S ou setas cima/baixo)
    private Vector3 direcao; // Direção de movimento

    // Pular e verificar colisão com chão
    [SerializeField]
    private float forcaPulo = 10f; // Força do pulo
    private Rigidbody rb; // Referência ao Rigidbody para física

    [SerializeField]
    private LayerMask layermask; // Máscara de camada para detectar o chão

    [SerializeField]
    private float groundCheckSize; // Tamanho da esfera de verificação de chão

    [SerializeField]
    private Vector3 groundCheckPosition; // Posição relativa da verificação de chão

    public bool isGrounded; // Indica se o personagem está no chão

    // Referência para a câmera
    [SerializeField]
    private CinemachineFreeLook cinemachineCamera; // Câmera do Cinemachine

    // Id do banco de dados
    [SerializeField]
    private int id; // Identificador usado para operações de banco de dados

    private void Awake()
    {
        BancoDeDados bancoDeDados = new BancoDeDados(); // Instancia o banco de dados
        bancoDeDados.CriarBanco(); // Cria o banco de dados
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no meio da tela
        Cursor.visible = false; // Torna o cursor invisível
        velocidade = veloAndando; // Define a velocidade inicial como a de andar
        Carregar(id); // Carrega a posição do jogador ao iniciar
    }

    void Update()
    {
        HandleMovement(); // Lida com a movimentação do personagem
        HandleActions(); // Lida com as ações do personagem (e.g., farpar)
        HandleSavingAndDeleting(); // Lida com salvar e deletar
    }

    private void HandleMovement()
    {
        Andar(); // Controla a movimentação ao andar
        Correr(); // Controla a movimentação ao correr
        Pular(); // Controla o pulo
    }

    private void HandleActions()
    {
        Farpar(); // Controla a ação de farpar (emote)
        ControlarCursor();
    }

    private void HandleSavingAndDeleting()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Salvar(id); // Salva a posição do jogador
            Debug.Log("Salvou"); // Mensagem de debug
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Deletar(id); // Deleta os dados do jogador
            Debug.Log("Deletou"); // Mensagem de debug
        }
    }

    private void Andar()
    {
        inputX = Input.GetAxis("Horizontal"); // Obtém a entrada horizontal
        inputZ = Input.GetAxis("Vertical"); // Obtém a entrada vertical

        Vector3 forward = cinemachineCamera.transform.forward; // Direção para frente da câmera
        forward.y = 0; // Ignora o eixo Y para manter o movimento horizontal
        forward.Normalize(); // Normaliza a direção

        Vector3 right = cinemachineCamera.transform.right; // Direção para a direita da câmera
        right.y = 0; // Ignora o eixo Y
        right.Normalize(); // Normaliza a direção

        direcao = (forward * inputZ + right * inputX).normalized; // Calcula a direção desejada

        if (direcao.magnitude >= 0.1f) // Verifica se há movimento significativo
        {
            transform.Translate(direcao * velocidade * Time.deltaTime, Space.World); // Move o personagem
            anim.SetBool("andando", true); // Ativa a animação de andar

            Quaternion toRotation = Quaternion.LookRotation(direcao, Vector3.up); // Calcula a rotação desejada
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                toRotation,
                5 * Time.deltaTime
            ); // Rotaciona suavemente
        }
        else
        {
            anim.SetBool("andando", false); // Desativa a animação de andar se não houver movimento
        }
    }

    private void Correr()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && direcao != Vector3.zero) // Verifica se a tecla Shift está pressionada
        {
            velocidade = veloCorrendo; // Define a velocidade de correr
            anim.SetBool("correndo", true); // Ativa a animação de correr
            anim.SetBool("andando", false); // Desativa a animação de andar
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || direcao == Vector3.zero) // Verifica se a tecla Shift foi solta ou não há movimento
        {
            velocidade = veloAndando; // Define a velocidade de andar
            anim.SetBool("correndo", false); // Desativa a animação de correr
        }
    }

    public void Pular(float multiplicador = 1f)
    {
        // Obtém a escala atual do objeto
        Vector3 escalaAtual = transform.localScale;

        // Calcula o tamanho e a posição da verificação de chão com base na escala do objeto
        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);
        // Realiza a verificação de chão com os valores escalados
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
        if (Input.GetKeyDown(KeyCode.E)) // Verifica se a tecla E foi pressionada
        {
            anim.SetTrigger("emote1"); // Ativa o trigger para a animação de farpar
        }
    }

    private void OnDrawGizmos()
    {
        // Obtém a escala atual do objeto
        Vector3 escalaAtual = transform.localScale;

        // Calcula o tamanho e a posição da verificação de chão com base na escala do objeto
        float tamanhoVerificado =
            groundCheckSize * Mathf.Max(escalaAtual.x, escalaAtual.y, escalaAtual.z);
        Vector3 posicaoVerificada = Vector3.Scale(groundCheckPosition, escalaAtual);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + posicaoVerificada, tamanhoVerificado);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("voltar")) // Verifica se o objeto colidido tem a tag "voltar"
            SceneManager.LoadScene("CenaInicial"); // Carrega a cena inicial
    }

    private void ControlarCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cursorBloqueado)
            {
                Cursor.lockState = CursorLockMode.None; // Libera o cursor
                Cursor.visible = true; // Mostra o cursor
                cursorBloqueado = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Bloqueia o cursor no centro da tela
                Cursor.visible = false; // Esconde o cursor
                cursorBloqueado = true;
            }
        }
    }

    private void Salvar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados(); // Instancia o banco de dados
        bancoDeDados.InserirPosicao(
            id,
            transform.position.x,
            transform.position.y,
            transform.position.z
        ); // Salva a posição
    }

    private void Deletar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados(); // Instancia o banco de dados
        bancoDeDados.NovoJogo(); // Deleta os dados do jogador
    }

    private void Carregar(int id)
    {
        BancoDeDados bancoDeDados = new BancoDeDados(); // Instancia o banco de dados
        var leitura = bancoDeDados.LerPosicao(id); // Lê a posição do banco de dados
        if (leitura != null)
        {
            while (leitura.Read()) // Lê os dados enquanto houver registros
            {
                transform.position = new Vector3(
                    leitura.GetFloat(1),
                    leitura.GetFloat(2),
                    leitura.GetFloat(3)
                ); // Define a posição do personagem
            }
        }
        leitura.Close(); // Fecha o leitor de dados
        bancoDeDados.FecharConexao(); // Fecha a conexão com o banco de dados
    }
}
