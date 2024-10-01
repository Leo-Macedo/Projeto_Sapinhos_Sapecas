using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeguirEAnimarNPC : MonoBehaviour
{
    public NavMeshAgent agent;    
    public Transform target;      
    private Animator animator;     

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }

        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("andou", true);
        }
        else if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("andou", false);
        }
    }
}

