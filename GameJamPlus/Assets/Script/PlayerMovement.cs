using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

    public bool isAttacking = false;
    public float attackCooldown = 0.5f;
    private float attackTimer = 0;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //animação de movimento
        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
            if (moveInput < 0)
                transform.localScale = new Vector3(-1, 1, 1); // Esquerda
            else
                transform.localScale = new Vector3(1, 1, 1);  // Direita
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        //cooldown de ataque
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                isAttacking = false;
                attackTimer = 0;
                animator.ResetTrigger("Attack");
            }
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            isGrounded = true;
            animator.ResetTrigger("Jump");
        }
    }

    
    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        //lógica para causar dano aos inimigos
        yield return new WaitForSeconds(attackCooldown);
    }
}
