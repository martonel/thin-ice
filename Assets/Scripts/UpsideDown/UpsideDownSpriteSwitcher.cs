using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownSpriteSwitcher : MonoBehaviour
{
    public SpriteRenderer normalSprite;
    public SpriteRenderer shadowSprite;
    public SpriteRenderer shadowSprite2;
    public bool isUpsideDown = false;

    public Collider2D[] enemyColliders;

    public GameObject enemyObj;


    private void Start()
    {
        //enemyColliders = colliderObj.gameObject.GetComponents<Collider2D>();
        if(enemyColliders == null || enemyColliders.Length ==0)
        {
            enemyColliders = GetComponentsInChildren<Collider2D>();
        }
    }
    public void SwitchSide()
    {
        isUpsideDown = !isUpsideDown;

        //Debug.Log("switch" + isUpsideDown);
        if (isUpsideDown)
        {
            normalSprite.color = new Color(normalSprite.color.r, normalSprite.color.g, normalSprite.color.b, 0.2f);
            if (shadowSprite != null)
            {
                shadowSprite.gameObject.SetActive(false);
            }
            shadowSprite2.gameObject.SetActive(false);

            StartCoroutine(ToggleColliders(false));


            if (enemyObj != null)
            {
                enemyObj.tag = "Untagged";
            }
        }
        else
        {
            normalSprite.color = new Color(normalSprite.color.r, normalSprite.color.g, normalSprite.color.b, 1f);

            if (shadowSprite != null)
            {
                shadowSprite.gameObject.SetActive(true);
            }
            shadowSprite2.gameObject.SetActive(true);

            StartCoroutine(ToggleColliders(true));

            if (enemyObj != null)
            {
                enemyObj.tag = "Enemy";
            }
        }
    }

    IEnumerator ToggleColliders(bool isEnabled)
    {
        Debug.Log("collider number" + enemyColliders.Length);

        foreach (var col in enemyColliders)
        {
            col.enabled = isEnabled;
            yield return null; // 1 frame
        }
    }


}
