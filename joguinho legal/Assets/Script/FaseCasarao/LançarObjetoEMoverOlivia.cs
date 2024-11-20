using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LançarObjetoEMoverOlivia : MonoBehaviour
{
    public GameObject caixaPrefab;
    public Transform spawnPoint;
    public float throwForce = 10f;
    public float rotationSpeed = 5f;
    public float moveSpeed = 3f;
    public List<Transform> waypoints;
    private GameObject caixa;
    private Transform player;
    private Animator animator;
    private int waypointAtual = -1;
    private Rigidbody rb;
    public bool podeLancar = false;
    public bool andando = false; // Variável para indicar se está andando
    public VidaVilao vidaVilao;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoverEntreWaypoints());
    }

    private IEnumerator MoverEntreWaypoints()
    {
        while (!vidaVilao.morreuvilao) // Continua enquanto o vilão não estiver morto
        {
            if (waypoints.Count == 0)
                yield break;

            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, waypoints.Count);
            } while (randomIndex == waypointAtual);

            waypointAtual = randomIndex;
            Transform targetWaypoint = waypoints[randomIndex];

            andando = true; // Define como andando ao começar a mover
            animator.SetBool("andando", true); // Atualiza o Animator

            while (
                Vector3.Distance(
                    new Vector3(transform.position.x, 0, transform.position.z),
                    new Vector3(targetWaypoint.position.x, 0, targetWaypoint.position.z)
                ) > 0.1f
            )
            {
                Vector3 direcao = new Vector3(
                    targetWaypoint.position.x - transform.position.x,
                    0, // Ignora a altura
                    targetWaypoint.position.z - transform.position.z
                ).normalized;

                transform.position += direcao * moveSpeed * Time.deltaTime;

                Quaternion lookRotation = Quaternion.LookRotation(direcao);
                lookRotation.x = 0;
                lookRotation.z = 0;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    Time.deltaTime * rotationSpeed
                );

                yield return null;
            }

            andando = false; // Define como parado ao atingir o destino
            animator.SetBool("andando", false); // Atualiza o Animator

            if (podeLancar)
            {
                yield return StartCoroutine(LançarCaixaPeriodicamente());
            }

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator LançarCaixaPeriodicamente()
    {
        if (caixa == null)
        {
            animator.SetTrigger("lançar");
            yield return StartCoroutine(RotacionarDuranteAnimacao());
        }
    }

    private IEnumerator RotacionarDuranteAnimacao()
    {
        float tempoAnimacao = animator.GetCurrentAnimatorStateInfo(0).length;
        float tempoDecorrido = 0f;

        while (tempoDecorrido < tempoAnimacao)
        {
            RotacionarParaJogador();
            tempoDecorrido += Time.deltaTime;
            yield return null;
        }
    }

    private void RotacionarParaJogador()
    {
        if (player != null)
        {
            Vector3 direcao = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direcao);
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    public void LançarCaixa()
    {
        caixa = Instantiate(caixaPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = caixa.GetComponent<Rigidbody>();
        Vector3 direcaoLançamento = (player.position - spawnPoint.position).normalized;
        rb.AddForce(direcaoLançamento * throwForce, ForceMode.Impulse);
        caixa.AddComponent<CaixaDanoNoPlayer>();
        caixa = null;
    }
}
