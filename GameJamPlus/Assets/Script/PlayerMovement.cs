using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

    public bool isAttacking = false;
    public float attackCooldown = 0.5f;
    private float attackTimer = 0;

    public int vida = 3;

    [SerializeField]
    private GameObject spawnpoint;
    [SerializeField]
    private Text spawnText;

    private Rigidbody2D rb;
    private Animator animator;
    public Collectible collectible;
    private bool liberar = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        spawnText.text = "Vida : " + vida;

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
        if (collision.gameObject.CompareTag("Agua"))
        {
            PerderVida(1);
            transform.position = spawnpoint.transform.position;
        }
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
            Debug.Log("pegiu");
            collectible.count++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            collectible.count++;
        }

        if (other.gameObject.CompareTag("Amigo"))
        {
            if(collectible.count >= 1)
            {
                collectible.count--;
                Destroy(other.gameObject);
                liberar = true;
                Destroy(other.gameObject);

            }
            
        }

        if (other.gameObject.CompareTag("Portal") && liberar)
        {
            SceneManager.LoadScene("NextScene");
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        //lógica para causar dano aos inimigos
        yield return new WaitForSeconds(attackCooldown);
    }

    public void PerderVida(int damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            vida = 0;
            Die();
        }
    }

    void Die()
    {
        transform.position = spawnpoint.transform.position;
    }
}
