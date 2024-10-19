using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed;
    private Transform player;

    public float attackRange;
    public int attackDamage = 10;  
    public float attackRate = 1.0f;
    private float nextAttackTime = 0f;

    public int enemyHealth = 3;
    public int currentHealth;

    private PlayerMovement playerHealth;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerMovement>();

        currentHealth = enemyHealth;
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector2 point = currentPoint.position - transform.position;
        animator.SetBool("Patrolling", true);
        animator.SetBool("Attacking", false);

        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void Attack()
    {
        animator.SetBool("Attacking", true);
        animator.SetBool("Patrolling", false);

        rb.velocity = Vector2.zero;

        if (Time.time >= nextAttackTime)
        {
            
            if (playerHealth != null)
            {
                playerHealth.PerderVida(attackDamage);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
