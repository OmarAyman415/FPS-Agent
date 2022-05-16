using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class scr_EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 50f;
    public float lookRadius = 10f;
    public float damage = 5f;
    public GameObject player;

    public bool playerInReach;
    public float attackDelayTimer;
    public float delayBetweenAttacks;
    public float attackAnimStartDelay;
    private bool isDead;


    Transform target;
    NavMeshAgent agent;

    public Animator enemyAnimator;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update ()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if(!isDead)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.isStopped = true;
            }

            if (distance <= agent.stoppingDistance)
            {
                playerInReach = true;
            }
            else
            {
                playerInReach = false;
            }
        }

        if (playerInReach)
        {
            Attack();
        }

        enemyAnimationPlay();

        attackDelayTimer += Time.deltaTime;


        if (attackDelayTimer >= 6.3f)
        {
            attackDelayTimer = 1f;
        }
    }

    public void Attack()
    {
        if (attackDelayTimer >= 1f)
        {
            player.GetComponent<scr_CharacterController>().TakeDamage(damage);
            attackDelayTimer = 0f;
        }
    }

    


    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            enemyAnimator.SetTrigger("isDead");
            Die();
            isDead = true;
        }
    }

    void Die()
    {
        Destroy(gameObject, 2f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void enemyAnimationPlay()
    {
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }

        if (attackDelayTimer >= delayBetweenAttacks - attackAnimStartDelay && attackDelayTimer <= delayBetweenAttacks && playerInReach)
        {
            enemyAnimator.SetBool("isAttacking", true);
        }
        else if (!playerInReach)
        {
            enemyAnimator.SetBool("isAttacking", false);
        }
    }
}
