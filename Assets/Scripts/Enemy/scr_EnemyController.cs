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

    public bool playerInReach;
    public float attackDelayTimer;
    public float delayBetweenAttacks;
    public float attackAnimStartDelay;
    private bool isDead;
    public int enemiesKilled = 0;


    Transform target;
    NavMeshAgent agent;

    
    scr_EnemyManager spawn;

    public Animator enemyAnimator;

    #region - Start -

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        spawn = GetComponent<scr_EnemyManager>();
    }

    #endregion

    #region - Update -

    void Update ()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if(!isDead)
            {
                agent.SetDestination(target.position);
                if (distance <= agent.stoppingDistance + 0.2f)
                {
                    Attack();
                    playerInReach = true;
                }
                else
                {
                    playerInReach = false;
                }
            }
            else
            {
                agent.isStopped = true;
            }
        }

        enemyAnimationPlay();

        attackDelayTimer += Time.deltaTime;


        if (attackDelayTimer >= 6.3f)
        {
            attackDelayTimer = 1f;
        }
    }

    #endregion

    #region - Attack -

    public void Attack()
    {
        if (attackDelayTimer >= 1f)
        {
            target.GetComponent<scr_CharacterController>().TakeDamage(damage);
            attackDelayTimer = 0f;
        }
    }

    #endregion

    #region - Taking Damage -

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
        enemiesKilled++;
        Destroy(GetComponent<BoxCollider>());
    }

    #endregion

    #region - Gizmo -

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    #endregion

    #region - Animation -

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

    #endregion
}
