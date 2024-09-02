using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalançarLinguada : MonoBehaviour
{
    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse1;
    public AudioSource somLinguada;

    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip,
        cam,
        player;
    public LayerMask whatIsGrappleable;
    private Animator animator;

    [Header("Swinging")]
    public float maxSwingDistance = 25f;
    public float pullSpeed = 10f; // Velocidade com que o jogador será puxado para o ponto
    private Vector3 currentGrapplePosition;

    private Vector3 swingPoint;
    private bool isPulling = false;

    [Header("Raycast Settings")]
    public float forwardOffset = 1.0f;
    public float upwardOffset = 1.5f;

    [Header("Line Settings")]
    public AnimationCurve tongueWidthCurve;
    public float tongueWidthMultiplier = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Configura a curva de largura da linha
        lr.widthCurve = tongueWidthCurve;
        lr.widthMultiplier = tongueWidthMultiplier;
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey))
            StartSwing();
        if (Input.GetKeyUp(swingKey))
            StopSwing();

        if (isPulling)
        {
            PullPlayer();
            animator.SetBool("voar", true); // Ativa a animação de swing
        }
        else
        {
            animator.SetBool("voar", false); // Desativa a animação de swing
        }
    }

    private void StartSwing()
    {
        RaycastHit hit;

        Vector3 offset = cam.forward * forwardOffset + cam.up * upwardOffset;
        Vector3 rayOrigin = cam.position + offset;

        if (Physics.Raycast(rayOrigin, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            swingPoint = hit.point;
            isPulling = true;
            somLinguada.Play();

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        isPulling = false;
    }

    void PullPlayer()
    {
        
        // Puxa o jogador em direção ao ponto de acerto
        player.position = Vector3.MoveTowards(
            player.position,
            swingPoint,
            pullSpeed * Time.deltaTime
        );

        // Se o jogador chegou no ponto de acerto, para de puxar
        if (Vector3.Distance(player.position, swingPoint) < 1.0f)
        {
            StopSwing();
        }
    }

    void DrawRope()
    {
        if (!isPulling)
            return;

        currentGrapplePosition = Vector3.Lerp(
            currentGrapplePosition,
            swingPoint,
            Time.deltaTime * 8f
        );

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
    }
}
