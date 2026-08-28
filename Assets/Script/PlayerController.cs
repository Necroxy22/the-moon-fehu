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
    public bool HasExtraJump => extraJumpsAvailable > 0;
    private PowerUp_Manager powerUpManager;

    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip damageSound;
    public AudioClip itemSound;

    void Start()
    {
        Time.timeScale = 1f; // reset guard

        rb = GetComponent<Rigidbody2D>();
        powerUpManager = GetComponent<PowerUp_Manager>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    private void Jump()
    {
        Debug.Log("Jump() called at " + Time.time);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void GrantDoubleJump()
    {
        extraJumpsAvailable = 1;
    }

    public float fallDeadZoneY = -15f;
    private bool diedFromAbyss = false;

    void Update()
    {
        if (!isDead && transform.position.y < fallDeadZoneY)
        {
            diedFromAbyss = true;
            currentHealth = 0;
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            Die();
            return;
        }

        if (isDead)
        {
            if (isFalling && !diedFromAbyss)
            {
                if (rb != null && rb.velocity.y < 0)
                {
                    rb.velocity += Vector2.up *
                        Physics2D.gravity.y *
                        (fallMultiplier - 1) *
                        Time.deltaTime;
                }

                bool grounded = groundCheck != null && Physics2D.OverlapCircle(
                    groundCheck.position,
                    groundCheckRadius,
                    groundLayer
                );

                if (grounded && (rb == null || rb.velocity.y <= 0.1f))
                {
                    isFalling = false;

                    if (rb != null)
                    {
                        rb.velocity = Vector2.zero;
                        rb.bodyType = RigidbodyType2D.Static;
                    }

                    if (animator != null)
                    {
                        animator.SetTrigger("Death");
                    }
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


        bool hasPegasus = powerUpManager != null && powerUpManager.HasPegasusEffect;
        bool canDoubleJump = (powerUpManager != null && powerUpManager.HasHermesEffect) || (extraJumpsAvailable > 0) || hasPegasus;
        int maxExtraJumps = hasPegasus ? 2 : 1;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                extraJumpsUsed = 0;
                PlaySound(jumpSound, 1f);
            }
            else if (canDoubleJump && extraJumpsUsed < maxExtraJumps)
            {
                Jump();
                extraJumpsUsed++;
                PlaySound(jumpSound, 1f);
            }//koyoasu
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

        UpdateCharacterAnimation();
    }

    private void UpdateCharacterAnimation()
    {
        if (animator == null || isDead) return;

        bool isJumpingUp = !isGrounded && rb != null && rb.velocity.y > 0.1f;

        string targetAnim = "Run";

        if (powerUpManager != null && powerUpManager.ActiveHeldPowerUp != null)
        {
            PowerUpType held = powerUpManager.ActiveHeldPowerUp.Value;
            switch (held)
            {
                case PowerUpType.Zeus:
                    targetAnim = isJumpingUp ? "JumpWithBintang" : "RunWithBintang";
                    break;
                case PowerUpType.Athena:
                    targetAnim = isJumpingUp ? "JumpWithShield" : "RunWithShield";
                    break;
                case PowerUpType.Hermes:
                case PowerUpType.Pegasus:
                    targetAnim = isJumpingUp ? "JumpWithSepatu" : "RunWithSepatu";
                    break;
            }
        }
        else
        {
            targetAnim = isJumpingUp ? "Jump" : "Run";
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnim))
        {
            animator.Play(targetAnim);
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
        else if (currentHealth == 1)
        {
            NotificationManager.Notif("Hati hati cok! darah lu sekarat");
        }
        else
        {
            PlaySound(damageSound, 1f);
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private void Die()
    {
        isDead = true;
        isFalling = true;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 4f;

            rb.velocity = new Vector2(0f, Mathf.Min(rb.velocity.y, -0.5f));
        }

        PlaySound(crashSound, 1f);

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        if (!diedFromAbyss)
        {
            yield return new WaitUntil(() => !isFalling);
            yield return null;
        }

        float clipLength = deathAnimationFallbackDuration;

        if (animator != null && !diedFromAbyss)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0 && clipInfo[0].clip != null)
            {
                clipLength = clipInfo[0].clip.length;
            }
        }
        else
        {
            clipLength = 0.2f;
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

    public void PlaySound(AudioClip clip, float volume = 1f)
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