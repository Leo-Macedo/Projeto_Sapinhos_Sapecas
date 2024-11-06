using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapangaMovimentacao : MonoBehaviour
{
    public Transform[] pontosCaminho;
    public float speed;
    private int pontoAtual = 0;
    private bool podeMover = true;

    private Animator anim;

    private bool andandoFrente = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (podeMover && pontoAtual < pontosCaminho.Length)
        {
            moveAtePonto(pontosCaminho[pontoAtual]);

            if (Vector3.Distance(transform.position, pontosCaminho[pontoAtual].position) < 1f)
            {
                if (andandoFrente)
                {
                    pontoAtual++;
                }
                else
                {
                    pontoAtual--;
                }

                if (pontoAtual == pontosCaminho.Length)
                {
                    andandoFrente = false;
                    pontoAtual--;
                    StartCoroutine(IntervaloParado());
                }

                if (pontoAtual == -1)
                {
                    andandoFrente = true;
                    pontoAtual++;
                    StartCoroutine(IntervaloParado());
                }
            }
        }
    }

    IEnumerator IntervaloParado()
    {
        anim.SetInteger("transition", 0);
        podeMover = false;

        yield return new WaitForSeconds(5f);

        podeMover = true;
    }

    private void moveAtePonto(Transform targetPoint)
    {
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        var rotation = Quaternion.LookRotation(targetPoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
        anim.SetInteger("transition", 1);
    }
}
