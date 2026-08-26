using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float jumpForce = 12f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 2f;
    public int maxHealth = 3;
    private int currentHealth;
    public float invulnerabilityTime = 2f;
    private bool isInvulnerable = false;
    public bool IsInvulnerable => isInvulnerable;
    public bool IsDead => isDead;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;
    public float deathAnimationFallbackDuration = 1f;

    private bool isDead = false;
    private bool isFalling = false;

    private int extraJumpsAvailable = 0;
    private int extraJumpsUsed = 0;

    private PowerUp_Manager powerUpManager;

    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip damageSound;
    public AudioClip itemSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        powerUpManager = GetComponent<PowerUp_Manager>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void GrantDoubleJump()
    {
        extraJumpsAvailable = 1;
    }

    void Update()
    {
        if (isDead)
        {
            if (isFalling)
            {
                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector2.up *
                        Physics2D.gravity.y *
                        (fallMultiplier - 1) *
                        Time.deltaTime;
                }

                bool grounded = Physics2D.OverlapCircle(
                    groundCheck.position,
                    groundCheckRadius,
                    groundLayer
                );

                if (grounded && rb.velocity.y <= 0.1f)
                {
                    isFalling = false;

                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Static;

                    animator.SetTrigger("Death");
                }
            }
            return;
        }

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                Jump();
                if (animator != null) animator.SetTrigger("Jump");
                extraJumpsUsed = 0;
                PlaySound(jumpSound, 1f);
            }
            else if (extraJumpsUsed < extraJumpsAvailable)
            {
                //tastistus
                Jump();
                if (animator != null) animator.SetTrigger("Jump");
                extraJumpsUsed++;
                extraJumpsAvailable = 0;
                PlaySound(jumpSound, 1f);
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                rb.velocity.y * 0.5f
            );
        }

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        float flashInterval = 0.1f;

        while (elapsed < invulnerabilityTime)
        {
            Color color = spriteRenderer.color;

            color.a = 0.3f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(flashInterval);

            color.a = 1f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(flashInterval);

            elapsed += flashInterval * 2f;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;

        isInvulnerable = false;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead)
            return;

        if (powerUpManager != null && powerUpManager.HasShield)
        {
            powerUpManager.ConsumeShield();
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private void Die()
    {
        isDead = true;
        isFalling = true;

        if (rb != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        PlaySound(crashSound, 1f);

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitUntil(() => !isFalling);

        yield return null;

        float clipLength = deathAnimationFallbackDuration;

        if (animator != null)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0 && clipInfo[0].clip != null)
            {
                clipLength = clipInfo[0].clip.length;
            }
        }

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(clipLength);

        if (GameOver_Manager.Instance != null)
        {
            GameOver_Manager.Instance.ShowGameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Collider2D myCollider = GetComponent<Collider2D>();
                if (myCollider != null && collision.collider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, collision.collider, true);
                }
            }
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            bool landedOnTop = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    landedOnTop = true;
                    break;
                }
            }

            if (landedOnTop)
            {
                extraJumpsUsed = 0;
            }
            else
            {
                TakeDamage(1);
            }
        }
    }

    public void PlayItemSound()
    {
        PlaySound(itemSound, 0.8f);
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (playerAudio != null && clip != null)
        {
            playerAudio.PlayOneShot(clip, volume);
        }
    }

    public int GetCurrentHP()
    {
        return currentHealth;
    }
}