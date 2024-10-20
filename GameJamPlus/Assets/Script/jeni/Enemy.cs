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
    public int enemyHealth = 1;

    private PlayerMovement playerHealth;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerMovement>();

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("playerAttack"))
        {
            TakeDamage();
        }
    }

    void Attack()
    {
        animator.SetBool("Attacking", true);
        animator.SetBool("Patrolling", false);

        rb.velocity = Vector2.zero;
        playerHealth.PerderVida();

    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }

    void Die()
    {
        Destroy(gameObject);
    }


}
