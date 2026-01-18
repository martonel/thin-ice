using UnityEngine;
using System.Collections;

public class LakeFroze : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.2f; // Mennyi ideig tartson az eltûnés
    private SpriteRenderer spriteRenderer;

    IEnumerator Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;

                // Kiszámoljuk az új alfa értéket (1-rõl 0-ra)
                float newAlpha = Mathf.Lerp(0.0f, 0.6f, elapsedTime / fadeDuration);

                // Alkalmazzuk az új színt
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

                // Várunk a következõ képkockáig
                yield return null;
            }
        }

        // 1. Várakozás 3 másodpercig
        yield return new WaitForSeconds(5.0f);

        // 2. Fokozatos eltûnés (Fade out)
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                // Kiszámoljuk az új alfa értéket (1-rõl 0-ra)
                float newAlpha = Mathf.Lerp(0.6f, 0f, elapsedTime / fadeDuration);

                // Alkalmazzuk az új színt
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

                // Várunk a következõ képkockáig
                yield return null;
            }
        }

        // 3. Objektum törlése
        Destroy(this.gameObject);
    }
}