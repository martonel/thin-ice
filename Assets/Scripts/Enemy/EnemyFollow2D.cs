using UnityEngine;

public class EnemyFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    public float detectionRange = 5f;
    public float moveSpeed = 3f;

    [Header("Detection")]
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;
    public float obstacleCheckDistance = 0.2f;

    [Header("Sprites")]
    public Sprite spriteA; // player alatt
    public Sprite spriteB; // player felett

    private bool isStopped = false;
    private bool isFalling = false;

    private Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererShade;

    [Header("enemyScore")]
    public int enemyScoreNumber;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (isFalling || player == null)
            return;

        CheckGround();

        HandleFlipAndSprite();

        if (!isStopped)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Move()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    // 🔁 FORDULÁS + SPRITE VÁLTÁS
    private void HandleFlipAndSprite()
    {
        // X tengely – fordulás
        if (player.position.x > transform.position.x)
        {
            if (player.position.y > transform.position.y)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
        }
        else
        {
            if (player.position.y > transform.position.y)
            {
                transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180, 0f);

            }
        }

        // Y tengely – sprite csere
        if (player.position.y > transform.position.y)
        {
            spriteRenderer.sprite = spriteB;
            spriteRendererShade.sprite = spriteB;
        }
        else
        {
            spriteRenderer.sprite = spriteA;
            spriteRendererShade.sprite = spriteA;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void CheckGround()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            0,
            groundLayer
        );

        if (groundHit.collider != null)
        {
            enemyFallIntoTheWater();
        }
    }

    void enemyFallIntoTheWater()
    {
        if (isFalling)
            return;

        isFalling = true;
        rb.gravityScale = 3f;
        anim.Play("EnemyFall");

        GameObject levelManagerObj = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManagerObj != null)
        {
            LevelManager levelManager = levelManagerObj.GetComponent<LevelManager>();
            if (levelManager != null)
            {
                levelManager.subFromEnemyOneNumber();
                levelManager.AddScore(enemyScoreNumber);
            }
        }
    }
}
