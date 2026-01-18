using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSlideMovement : MonoBehaviour
{
    public float steerForce = 15f;        // folyamatos "h�z�" er�
    public float maxSpeed = 5f;           // max sebess�g
    public float speedDamping = 0.99f;    // j�g �rzet (1 = v�gtelen cs�sz�s)
    public float bounceBackForce = 4f;    // falr�l lepattan�s ereje

    private Rigidbody2D rb;
    private bool isGameOver = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        if (Mouse.current.leftButton.isPressed)
        {
            SteerTowardsMouse();
        }
    }

    void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }
        // enyhe cs�sz�s-lassul�s
        rb.linearVelocity *= speedDamping;

        // sebess�g limit
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void SteerTowardsMouse()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - transform.position);
        if (dir.sqrMagnitude < 0.01f) return;

        dir.Normalize();

        rb.AddForce(dir * steerForce * Time.deltaTime, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(normal * bounceBackForce, ForceMode2D.Impulse);
    }

    internal void isEnd()
    {
        isGameOver = true;
        rb.linearVelocityX = 0f;
        rb.linearVelocityY = 0f;
    }

    public void IsMoveableAgain()
    {
        isGameOver = false;
    }
}
