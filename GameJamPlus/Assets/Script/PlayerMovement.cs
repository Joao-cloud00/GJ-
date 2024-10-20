using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

    public bool isAttacking = false;
    public float attackCooldown = 0.2f;
    private float attackTimer = 0;

    public int vida = 3;

    [SerializeField]
    private GameObject spawnpoint;
    [SerializeField]
    private TextMeshProUGUI spawnText;

    private Rigidbody2D rb;
    private Animator animator;
    public Collectible collectible;
    private bool liberar = false;

    public GameObject attackHitbox;
    public GameObject placaText;
    public Sprite gradeQuebrada;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackHitbox.SetActive(false);
        placaText.SetActive(false);
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if(vida == 0)
        {
            SceneManager.LoadScene(0);
        }

        spawnText.text = "Vida : " + vida;

        //anima��o de movimento
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
            AudioManager.Instance.PlaySFX("jump");
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(Attack());
            AudioManager.Instance.PlaySFX("attack");
        }

        //cooldown de ataque
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                isAttacking = false;
                attackTimer = 0;
                attackHitbox.SetActive(false);
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
            PerderVida();
            AudioManager.Instance.PlaySFX("death");
            transform.position = spawnpoint.transform.position;
        }
        if (collision.gameObject.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
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

        if (other.gameObject.CompareTag("Placa"))
        {
            placaText.SetActive(true);
            collectible.count++;
        }
        

        if (other.gameObject.CompareTag("Amigo"))
        {
            if(collectible.count >= 3)
            {
                collectible.count--;
                liberar = true;

                SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null && gradeQuebrada != null)
                {
                    spriteRenderer.sprite = gradeQuebrada;
                }
                

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
        attackHitbox.SetActive(true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
    }

    public void PerderVida()
    {
        vida--;
        transform.position = spawnpoint.transform.position;
        //vida -= damage;
        //if (vida <= 0)
        //{
        //   vida = 0;
        //  Die();
        //  }
    }

    void Die()
    {
        transform.position = spawnpoint.transform.position;
    }
}
