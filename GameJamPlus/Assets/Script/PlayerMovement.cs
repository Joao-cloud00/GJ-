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

    public Image[] coracoes; 
    public Sprite coracaoVazio;
    public Slider slider;
    private float groundCheckCooldown = 0.1f;
    private float lastJumpTime = 0f;


    private float maxGroundAngle = 30f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackHitbox.SetActive(false);
        placaText.SetActive(false);
        slider.maxValue = 3;
        slider.value = 0;
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
            lastJumpTime = Time.time;
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
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Verifica o ângulo da superfície de contato
                float angle = Vector2.Angle(contact.normal, Vector2.up);

                // Se o ângulo for menor que o ângulo máximo do chão, permite pular
                if (angle < maxGroundAngle && rb.velocity.y <= 0)
                {
                    isGrounded = true;
                    animator.ResetTrigger("Jump");
                }
            }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Verifica o ângulo da superfície de contato
                float angle = Vector2.Angle(contact.normal, Vector2.up);

                // Se o ângulo for menor que o ângulo máximo do chão, permite pular
                if (angle < maxGroundAngle && rb.velocity.y <= 0 && Time.time > lastJumpTime + groundCheckCooldown)
                {
                    isGrounded = true;
                    animator.ResetTrigger("Jump");
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            collectible.count++;
            AudioManager.Instance.PlaySFX("item");
            slider.value = collectible.count;
        }

        if (other.gameObject.CompareTag("Placa"))
        {
            placaText.SetActive(true);
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
            SceneManager.LoadScene(2);
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
