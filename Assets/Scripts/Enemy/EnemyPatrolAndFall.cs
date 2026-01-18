using UnityEngine;

public class EnemyPatrolAndFall : MonoBehaviour
{
    [Header("Movement")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Detection")]
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;
    public float obstacleCheckDistance = 0.2f;

    private Transform target;
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
        target = pointB;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (isFalling)
            return;

        CheckObstacle();
        CheckGround();

        if (!isStopped)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Move()
    {
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, target.position) < 0.05f)
        {
            target = target == pointA ? pointB : pointA;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            spriteRendererShade.flipX = !spriteRendererShade.flipX;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player ne tolja el
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // folyamatosan null�zzuk, �gy nem lassul / nem tol�dik
            rb.linearVelocity = Vector2.zero;
        }
    }

    void CheckObstacle()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            obstacleCheckDistance,
            obstacleLayer
        );

        isStopped = hit.collider != null;
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
