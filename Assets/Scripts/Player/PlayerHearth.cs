using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHearth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int maxHeartNumber = 4;
    public int hearthNumber = 4;

    public List<GameObject> heartObjects;
    public Animator anim;
    public SpriteBlinkAlpha spriteBlinkAlpha;

    private bool isDamaged = false;
    private void Start()
    {
        maxHeartNumber = hearthNumber;
        spriteBlinkAlpha = GetComponentInChildren<SpriteBlinkAlpha>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && !isDamaged)
        {
            Debug.Log("collide!" + collision.name + " - " + collision.tag);
            if(collision.tag.Equals("Enemy") && hearthNumber != 0){
                hearthNumber--;
                spriteBlinkAlpha.StartBlink();
                isDamaged=true;

                heartObjects[hearthNumber].SetActive(false);
                if (hearthNumber == 0)
                {
                    Debug.Log("Játék vége");
                    Time.timeScale = 0;
                    anim.Play("GameOverAnim");
                }

            }
        }
    }

    public void SetDamagable()
    {
        isDamaged = false;
    }

    public void AddHealth()
    {
        if (hearthNumber < maxHeartNumber)
        {
            heartObjects[hearthNumber].SetActive(true);
            hearthNumber++;
        }
    }
}
