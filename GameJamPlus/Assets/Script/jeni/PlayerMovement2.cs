using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;
    private int jumpCount; 

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
    public Sprite gradeQuebrada;

    public Image[] coracoes;
    public Sprite coracaoVazio;
    public Slider slider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackHitbox.SetActive(false);
        slider.maxValue = 3;
        slider.value = 0;
        jumpCount = 0; 
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        slider.value = collectible.count;

        if (vida == 0)
        {
            SceneManager.LoadScene(0);
        }

        spawnText.text = "Vida : " + vida;

        // Animação de movimento
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

        // Pulo
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < 1) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
                AudioManager.Instance.PlaySFX("jump");

                if (!isGrounded) 
                {
                    jumpCount++;
                }

                isGrounded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(Attack());
            AudioManager.Instance.PlaySFX("attack");
        }

        // Cooldown de ataque
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
            jumpCount = 0; 
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
            slider.value = collectible.count;
        }

        if (other.gameObject.CompareTag("Amigo"))
        {
            if (collectible.count >= 3)
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
        AtualizarVidaUI();
        transform.position = spawnpoint.transform.position;
    }

    void Die()
    {
        transform.position = spawnpoint.transform.position;
    }

    void AtualizarVidaUI()
    {
        for (int i = 0; i < coracoes.Length; i++)
        {
            if (i < vida)
            {
                coracoes[i].enabled = true;
            }
            else
            {
                coracoes[i].sprite = coracaoVazio;
            }
        }
    }
}
