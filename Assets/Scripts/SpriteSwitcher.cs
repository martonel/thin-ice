using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteSwitcher : MonoBehaviour
{
    [Header("Sprites")]
    public List<Sprite> sprites; // The sprite for 0 degrees
     // The sprite for 180 degrees

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        // Detect Left Mouse Button Click
        if (Mouse.current.leftButton.isPressed)
        {
            UpdateSpriteBasedOnAngle();
        }
    }

    void UpdateSpriteBasedOnAngle()
    {
        // 1. Get Mouse Position in World Space
        Vector3 mouseScreen = Mouse.current.position.ReadValue();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreen);
        mousePosition.z = 0f;

        // 2. Calculate Direction Vector from the object to the mouse
        Vector2 direction = mousePosition - transform.position;

        // 3. Calculate Angle in degrees
        // Atan2 returns the angle in radians, so we convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust angle to be in the 0-360 range for easier comparison
        if (angle < 0) angle += 360f;


        //Debug.Log("angle" + angle);
        // 4. Logic for Sprite Swapping
        // Using Mathf.Approximately or a small threshold is safer than "==" 
        // because float calculations can have tiny rounding errors.
        //0 jobb -> 337.5 és 22.5 között
        // 45 jobb fel -> 22.5 és 67.5 között
        //90 fel -> 67.5 és 112.5 között
        //135 bal fel -> 112.5 és 157.5 között 
        //180 bal -> 157.5 és 202.5 között
        //225 bal le -> 202.5 és 247.5 között
        //270 le -> 247.5 és 292.5 között
        //315 jobb le -> 292.5 és 337.5 között
        //360 jobb -> 337.5 és 22.5 között

        if (angle > 337.5 || angle < 22.5)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (angle > 22.5 && angle < 67.5)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            spriteRenderer.sprite = sprites[2];
        }
        else if (angle > 112.5 && angle < 157.5)
        {
            spriteRenderer.sprite = sprites[3];
        }
        else if (angle > 157.5 && angle < 202.5)
        {
            spriteRenderer.sprite = sprites[4];
        }
        else if (angle > 202.5 && angle < 247.5)
        {
            spriteRenderer.sprite = sprites[5];
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            spriteRenderer.sprite = sprites[6];
        }
        else if (angle > 292.5 && angle < 337.5)
        {
            spriteRenderer.sprite = sprites[7];
        }
        
    }
    private bool isGameOver = false;
    public void IsEnd()
    {
        Debug.Log("sprite end");
        //isGameOver = true;
        spriteRenderer.sprite = sprites[8];

    }
}